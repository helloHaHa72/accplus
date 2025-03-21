using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedgerController : _AbsAuthenticatedController
    {
        private readonly MainDbContext _context;
        private readonly ILedgersRepo _ledgerRepo;
        private readonly IledgerService _ledgerService;
        private readonly ILedgerTypesRepo _ledgerTypeRepo;
        public LedgerController(MainDbContext context,
            ILedgersRepo LedgerRep,
            IledgerService ledgerService,
            ILedgerTypesRepo ledgerTypeRepo)
        {
            _context = context;
            _ledgerRepo = LedgerRep;
            _ledgerService = ledgerService;
            _ledgerTypeRepo = ledgerTypeRepo;
        }

        [HttpGet("GetList")]
        public virtual async Task<IEnumerable<VMLedger>> GetList()
        {
            var resultList = await _ledgerRepo.GetList().Include(x => x.led05ledger_types)
                .OrderByDescending(x => x.DateCreated)
                .AsNoTracking()
            .ToListAsync();

            var newLedgers = resultList.Select(ledger => new VMLedger
            {
                ID = ledger.led01uin,
                LedgerName = ledger.led01title,
                Code = ledger.led01code,
                LedgerNameCode = ledger.led01title + "[" + ledger.led01code + "]",
                GLType = (GLType)ledger.led01led05uin,
                GLTypeName = ledger.led05ledger_types.led05title,
                Description = ledger.led01desc,
                ParentGLID = ledger.led01led03uin ?? 0,
                ParentGLName = _context.led03general_ledgers.FirstOrDefault(l => l.led03uin == ledger.led01led03uin)?.led03title,
                Balance = ledger.led01balance,
                Status = ledger.led01status,
                IsDefault = ledger.led01isdefaultled ?? false
            });

            return newLedgers;
        }

        [HttpGet("GetStatement/{ledgerCode}")]
        public virtual async Task<IEnumerable<VMUserStatement>> GetStatement(string ledgerCode)
        {
            var ledgerWithVouchers = await _ledgerRepo.GetList()
                .Include(x => x.led05ledger_types)
                .Include(x => x.vou03voucher_details)
                .Where(x => x.led01code.Contains(ledgerCode))
                .OrderByDescending(x => x.DateCreated)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (ledgerWithVouchers == null)
            {
                return Enumerable.Empty<VMUserStatement>();
            }

            var statements = ledgerWithVouchers.vou03voucher_details.Select(voucher => new VMUserStatement
            {
                VoucherNo = voucher.vou03vou02full_no,
                Description = voucher.vou03description,
                Debit = voucher.vou03dr,
                Credit = voucher.vou03cr,
                Balance = voucher.vou03balance,
                ChequeNo = voucher.vou03chq,
                Date = voucher.DateCreated
            });

            return statements;
        }

        [HttpGet("ListLedgers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLedgerList(string data)
        {
            var fetchData = await _ledgerService.FetchDefaultLedgerDate(data);

            return Ok(fetchData);
        }

        [HttpGet("ListLedgersTypes")]
        public async Task<IActionResult> GetLedgerTypeList()
        {
            var fetchData = await _ledgerTypeRepo.GetList()
                .Select(l => new VMLedgerTypes
                {
                    Id = l.led05uin,
                    Title = l.led05title
                })
                .ToListAsync();

            return Ok(fetchData);
        }


        [HttpGet("GetDetail/{id}")]
        public virtual async Task<IActionResult> GetDetail(int id)
        {
            var resultList = await _ledgerRepo.GetList().Include(x => x.led05ledger_types)
                .Where(x => x.led01uin == id)
                .OrderByDescending(x => x.DateCreated)
                .AsNoTracking()
            .FirstOrDefaultAsync();


            if (resultList == null)
            {
                return NotFound("Ledger not found");
            }

            VMLedger ledgerDetail = new VMLedger
            {
                ID = resultList.led01uin,
                LedgerName = resultList.led01title,
                //Added code
                LedgerNameCode = resultList.led01title + "[" + resultList.led01code + "]",
                Code = resultList.led01code,
                GLType = (GLType)resultList.led01led05uin,
                GLTypeName = resultList.led05ledger_types.led05title,
                Description = resultList.led01desc,
                ParentGLID = resultList.led01led03uin ?? 0,
                ParentGLName = _context.led03general_ledgers.FirstOrDefault(l => l.led03uin == resultList.led01led03uin)?.led03title,
                Balance = resultList.led01balance,
                Status = resultList.led01status,
                IsDefault = resultList.led01isdefaultled ?? false,
            };

            return Ok(ledgerDetail);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(VMLedger ledger)
        {
            if (ledger == null)
            {
                return BadRequest("Invalid data.");
            }

            var generaLedger = _context.led03general_ledgers.FirstOrDefault(l => l.led03uin == ledger.ParentGLID);

            if (generaLedger == null)
            {
                return BadRequest("GeneralLedger not found.");
            }

            var ledgerType = _context.led05ledger_types.FirstOrDefault(l => l.led05title == ledger.GLType.ToString());

            if (ledgerType == null)
            {
                return BadRequest("LedgerType not found.");
            }

            await RevokePreviousDefaultLedger(ledger);

            var ledgerEntity = new led01ledgers
            {
                led01led03uin = ledger.ParentGLID,
                led01led05uin = ledgerType.led05uin,
                led01title = ledger.LedgerName,
                led01code = ledger.Code,
                led01desc = ledger.Description,
                led01isdefaultled = ledger.IsDefault,

                led01status = true,
                led01deleted = false,
                CreatedName = _ActiveUserName,
                DateCreated = DateTime.Now,
                UpdatedName = " ",
                DateUpdated = DateTime.Now,
                DeletedName = " ",
                led01balance = 0

            };

            _context.led01ledgers.Add(ledgerEntity);
            _context.SaveChanges();

            return Ok("Ledger created successfully.");
        }

        private async Task RevokePreviousDefaultLedger(VMLedger ledger)
        {
            if (ledger.IsDefault)
            {
                var ledData = await _ledgerRepo.GetList().Where(x => x.led01led03uin == ledger.ParentGLID && x.led01isdefaultled == true).ToListAsync();

                if (ledData != null)
                {
                    foreach (var led in ledData)
                    {
                        led.led01isdefaultled = false;

                        _context.led01ledgers.Update(led);
                    }

                    _context.SaveChanges();
                }
            }
        }

        [HttpGet, Route("GetDefaultLedger/{parentGlId}")]
        public async Task<IActionResult> GetDefaultLedger(int parentGlId)
        {
            var ledData = await _ledgerRepo.GetList().Include(x => x.led05ledger_types).Where(x => x.led01led03uin == parentGlId && x.led01isdefaultled == true).FirstOrDefaultAsync();
            if (ledData == null) 
            {
                return Ok("Has no default.");
            }
            else
            {
                VMLedger ledgerDetail = new VMLedger
                {
                    ID = ledData.led01uin,
                    LedgerName = ledData.led01title,
                    //Added code
                    LedgerNameCode = ledData.led01title + "[" + ledData.led01code + "]",
                    Code = ledData.led01code,
                    GLType = (GLType)ledData.led01led05uin,
                    GLTypeName = ledData.led05ledger_types.led05title,
                    Description = ledData.led01desc,
                    IsDefault = ledData.led01isdefaultled ?? false,
                    ParentGLID = ledData.led01led03uin ?? 0,
                    ParentGLName = _context.led03general_ledgers.FirstOrDefault(l => l.led03uin == ledData.led01led03uin)?.led03title,
                    Balance = ledData.led01balance,
                    Status = ledData.led01status
                };

                return Ok(ledgerDetail);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, VMLedger ledger)
        {
            if (id != ledger.ID)
            {
                return BadRequest("Invalid ID.");
            }

            var oldData = await _ledgerRepo.GetDetailAsync(id);
            if (oldData == null)
            {
                return BadRequest("Ledger not found.");
            }

            var generaLedger = await _ledgerRepo.GetGeneralLedgerByUinAsync(ledger.ParentGLID);
            if (generaLedger == null)
            {
                return BadRequest("GeneralLedger not found.");
            }

            var ledgerType = await _ledgerRepo.GetLedgerTypeByTitleAsync(ledger.GLType.ToString());
            if (ledgerType == null)
            {
                return BadRequest("LedgerType not found.");
            }

            // Update properties of the existing ledger entity
            oldData.led01led03uin = ledger.ParentGLID;
            oldData.led01led05uin = ledgerType.led05uin;
            oldData.led01title = ledger.LedgerName;
            oldData.led01code = ledger.Code;
            oldData.led01desc = ledger.Description;
            oldData.led01status = ledger.Status;
            oldData.led01isdefaultled = ledger.IsDefault;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;

            // Perform any other updates as needed

            _ledgerRepo.Update(oldData);
            await _ledgerRepo.SaveAsync();

            return Ok("Ledger updated successfully.");
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var oldData = await _ledgerRepo.GetDetailAsync(id);
            if (oldData == null) { throw new Exception("Invalid Product ID"); }
            oldData.DateDeleted = DateTime.UtcNow;

            _ledgerRepo.Delete(oldData);
            await _ledgerRepo.SaveAsync();
            return Ok("Deleted Successfully");

        }
    }
}
