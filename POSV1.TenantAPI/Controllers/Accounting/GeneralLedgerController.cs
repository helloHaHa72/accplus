using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralLedgerController : _AbsAuthenticatedController
    {
        private readonly IGLedgersRepo _GledgerRepo;
        private readonly ILedgerTypesRepo _LedgerTypesRepo;
        private readonly MainDbContext _context;

        public GeneralLedgerController(IGLedgersRepo GLedgerRepo,
            MainDbContext context,
            ILedgerTypesRepo ledgerTypesRepo)
        {
            _GledgerRepo = GLedgerRepo;
            _context = context;
            _LedgerTypesRepo = ledgerTypesRepo;
        }

        [HttpGet("GetList")]
        public virtual async Task<IEnumerable<VMGeneralLedger>> GetList()
        {
            var resultList = await _GledgerRepo.GetList()
                .Include(x => x.led05ledger_types)
                .OrderByDescending(X => X.DateCreated)
                .AsNoTracking()
                .ToListAsync();

            var GLedgers = resultList.Select(gledger => new VMGeneralLedger
            {
                ID = gledger.led03uin,
                GLName = gledger.led03title,
                Code = gledger.led03code,
                GLedgerNameCode = gledger.led03title + "[" + gledger.led03code + "]",
                GLType = (GLType)gledger.led03led05uin,
                GlTypeName = gledger.led05ledger_types.led05title,
                Description = gledger.led03desc,
                ParentGLID = gledger.led03led03uin ?? 0,
                ParentGLName = _context.led03general_ledgers.FirstOrDefault(l => l.led03uin == gledger.led03led03uin)?.led03title,
                Status = gledger.led03status
            });

            return GLedgers;
        }

        //[HttpGet("GetList/LedgerType")]
        //public virtual async Task<IEnumerable<led05ledger_types>> GetLedgerTypeList()
        //{
        //    try
        //    {
        //        var resultList = await _LedgerTypesRepo.GetList().ToListAsync();
        //        return resultList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception as needed
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return Enumerable.Empty<led05ledger_types>(); // Return an empty list in case of an error
        //    }
        //}

        //[HttpPut("Update/LedgerType")]
        //public virtual async Task<IActionResult> UpdateLedgerType([FromBody] LedgerTypeDto ledgerTypeDto)
        //{
        //    if (ledgerTypeDto == null || ledgerTypeDto.Led05Uin <= 0)
        //    {
        //        return BadRequest("Invalid payload data.");
        //    }

        //    try
        //    {
        //        var existingLedgerType = await _LedgerTypesRepo.GetDetailAsync(ledgerTypeDto.Led05Uin);
        //        if (existingLedgerType == null)
        //        {
        //            return NotFound($"Ledger type with ID {ledgerTypeDto.Led05Uin} not found.");
        //        }

        //        // Update the existing ledger type with the new values from the DTO
        //        existingLedgerType.led05title = ledgerTypeDto.Led05Title;
        //        existingLedgerType.led05title_nep = ledgerTypeDto.Led05TitleNep;
        //        existingLedgerType.led05add_dr = ledgerTypeDto.Led05AddDr;
        //        existingLedgerType.UpdatedName = ledgerTypeDto.UpdatedName;
        //        existingLedgerType.DateUpdated = DateTime.UtcNow;

        //        _LedgerTypesRepo.Update(existingLedgerType); // Assuming this updates the database
        //        await _LedgerTypesRepo.SaveAsync();
        //        return Ok(existingLedgerType);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception as needed
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return StatusCode(500, "An internal server error occurred.");
        //    }
        //}




        [HttpGet("GetDetail/{id}")]
        public virtual async Task<IActionResult> GetDetail(int id)
        {
            var resultList = await _GledgerRepo.GetDetailAsync(id);


            if (resultList == null)
            {
                return NotFound("Ledger not found");
            }


            VMGeneralLedger ledgerDetail = new VMGeneralLedger
            {
                ID = resultList.led03uin,
                GLName = resultList.led03title,
                Code = resultList.led03code,
                GLType = (GLType)resultList.led03led05uin,
                Description = resultList.led03desc,
                ParentGLID = resultList.led03led03uin ?? 0,
                ParentGLName = resultList != null ? resultList.led03title : null,
                Status = resultList.led03status
            };

            return Ok(ledgerDetail);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] VMCreateGeneralLedger Data)
        {
            try
            {
                led03general_ledgers l1 = new led03general_ledgers();
                l1.led03title = Data.GLName;
                l1.led03desc = Data.Description;
                l1.led03code = Data.Code;
                l1.led03led05uin = (int)Data.GLType;


                if (Data.ParentGLID > 0)
                {
                    var generaLedger = await _GledgerRepo.GetDetailAsync(Data.ParentGLID ?? 0);

                    if (generaLedger == null)
                    {
                        return BadRequest("GeneralLedger not found.");
                    }
                    l1.led03led03uin = Data.ParentGLID;
                }

                l1.led03status = true;
                l1.led03deleted = false;
                l1.CreatedName = _ActiveUserName;
                l1.DateCreated = DateTime.Now;
                l1.UpdatedName = " ";
                l1.DateUpdated = DateTime.Now;
                l1.DeletedName = " ";
                l1.led03balance = "0";

                _GledgerRepo.Insert(l1);
                await _GledgerRepo.SaveAsync();
                return Ok("Created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get data. {ex.Message}");
            }
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VMCreateGeneralLedger Data)
        {
            var existingLedger = await _GledgerRepo.GetDetailAsync(id);

            if (existingLedger == null)
            {
                return BadRequest("GeneralLedger not found.");
            }

            existingLedger.led03title = Data.GLName;
            existingLedger.led03desc = Data.Description;
            existingLedger.led03code = Data.Code;
            existingLedger.led03led05uin = (byte)Data.GLType;

            if (Data.ParentGLID > 0)
            {
                var generaLedger = await _GledgerRepo.GetDetailAsync(Data.ParentGLID ?? 0);

                if (generaLedger == null)
                {
                    return BadRequest("Parent GeneralLedger not found.");
                }

                existingLedger.led03led03uin = Data.ParentGLID;
            }

            existingLedger.led03status = Data.Status;

            existingLedger.DateUpdated = DateTime.Now;
            existingLedger.UpdatedName = _ActiveUserName;

            _GledgerRepo.Update(existingLedger);
            await _GledgerRepo.SaveAsync();
            return Ok("Updated Successfully");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var oldData = await _GledgerRepo.GetDetailAsync(id);
            if (oldData == null) { throw new Exception("Invalid Product ID"); }
            oldData.DateDeleted = DateTime.UtcNow;

            _GledgerRepo.Delete(oldData);
            await _GledgerRepo.SaveAsync();
            return Ok("Deleted Successfully");

        }


    }
}
