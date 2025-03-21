using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;
using POSV1.TenantAPI.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace POSV1.TenantAPI.Controllers.Accounting
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly MainDbContext _context;
        // Define event delegates
        public delegate bool VoucherStatusChangedEventHandler(vou02voucher_summary Data);

        public event VoucherStatusChangedEventHandler VoucherApproved;
        public event VoucherStatusChangedEventHandler VoucherRejected;
        public event VoucherStatusChangedEventHandler VoucherUnApproved;
        public VoucherController(MainDbContext context)
        {
            _context = context;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateVoucherNumber(string prefix, string branchPrefix, int sn)
        {
            string paddedSn = sn.ToString().PadLeft(4, '0');

            return $"{prefix}-{branchPrefix}-{paddedSn}";
        }

        [HttpGet]
        public int GetNextSerialNumber()
        {
            var lastVoucher = _context.vou02voucher_summary.OrderByDescending(v => v.vou02full_no).FirstOrDefault();
            if (lastVoucher == null)
            {
                return 1;
            }

            string[] parts = lastVoucher.vou02full_no.Split('-');
            if (parts.Length >= 3 && int.TryParse(parts[2], out int serialNumber))
            {
                return serialNumber + 1;
            }
            else
            {
                return 1;
            }
        }


        [HttpPost]
        [Route("Create")]
        public IActionResult Create(VMVoucher voucher)
        {
            try
            {
                string prefix = "VOU";
                string branchPrefix = "BR01";

                if (voucher == null)
                {
                    return BadRequest("Invalid data.");
                }

                var voucherEntity = new vou02voucher_summary
                {
                    vou02full_no = GenerateVoucherNumber(prefix, branchPrefix, GetNextSerialNumber()),
                    vou02vou01uin = (int)EnumVoucherTypes.Journal,
                    vou02amount = voucher.Amount,
                    vou02description = voucher.Remarks,
                    vou02manual_vno = voucher.ManualVno,
                    vou02value_date = voucher.ValueDate,
                    vou02status = (EnumVoucherStatus)voucher.Status,
                    vou02chq = voucher.ChqNo,
                    vou03voucher_details = new List<vou03voucher_details>(),
                };

                if (voucher.VMVoucherDetailCreate != null)
                {
                    foreach (var voucher_Detail in voucher.VMVoucherDetailCreate)
                    {
                        var ledger = _context.led01ledgers.FirstOrDefault(l => l.led01uin == voucher_Detail.LedgerId);

                        if (ledger == null)
                        {
                            return BadRequest("Ledger not found.");
                        }

                        var voucherDetailEntity = new vou03voucher_details
                        {
                            vou03led05uin = ledger.led01uin,
                            vou03dr = voucher_Detail.Debit,
                            vou03cr = voucher_Detail.Credit,
                            vou03description = voucher_Detail.Description,
                            vou03chq = voucher_Detail.ChqNo,
                            vou03balance = ledger.led01balance,
                        };

                        voucherEntity.vou03voucher_details.Add(voucherDetailEntity);
                    }
                }

                _context.vou02voucher_summary.Add(voucherEntity);
                _context.SaveChanges();

                return Ok("Voucher created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get data. {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetVoucherList")]
        public async Task<IActionResult> GetVoucherList()
        {
            IQueryable<vou02voucher_summary> _Que = _context
                    .vou02voucher_summary;
            if (_Que == null)
            {
                return NotFound("No vouchers found.");
            }

            IList<VMVoucherList> Result = await _Que.Select(x => new VMVoucherList()
            {
                VoucherNo = x.vou02full_no,
                VoucherType = x.vou01voucher_types.vou01title,
                ValueDate = x.vou02value_date,
                ManualVno = x.vou02manual_vno,
                Remarks = x.vou02description,
                TotalCredit = x.vou03voucher_details.Sum(d => d.vou03cr),
                TotalDebit = x.vou03voucher_details.Sum(d => d.vou03dr),
                Status = x.vou02status.ToString(),
                UpdatedName = "Admin"

            })
              .ToListAsync();

            return Ok(Result);
        }

        [HttpGet]
        [Route("GetJournalVoucherList")]
        public async Task<IActionResult> GetJournalVoucherList()
        {
            IQueryable<vou02voucher_summary> _Que = _context
                    .vou02voucher_summary
                    .OrderByDescending(x => x.DateCreated)
                    .Where(x => x.vou01voucher_types.vou01title == "Journal");
            if (_Que == null)
            {
                return NotFound("No Journal vouchers found.");
            }

            IList<VMVoucherList> Result = await _Que.Select(x => new VMVoucherList()
            {
                VoucherNo = x.vou02full_no,
                VoucherType = x.vou01voucher_types.vou01title,
                ValueDate = x.vou02value_date,
                ManualVno = x.vou02manual_vno,
                Remarks = x.vou02description,
                TotalCredit = x.vou03voucher_details.Sum(d => d.vou03cr),
                TotalDebit = x.vou03voucher_details.Sum(d => d.vou03dr),
                Status = x.vou02status.ToString(),
                UpdatedName = "Admin"

            })
              .ToListAsync();

            return Ok(Result);
        }

        [HttpGet]
        [Route("GetIncomeVoucherList")]
        public async Task<IActionResult> GetIncomeVoucherList()
        {
            IQueryable<vou02voucher_summary> _Que = _context
                    .vou02voucher_summary
                    .OrderByDescending(x => x.DateCreated)
                    .Where(x => x.vou01voucher_types.vou01title == "Income");
            if (_Que == null)
            {
                return NotFound("No Journal vouchers found.");
            }

            IList<VMVoucherList> Result = await _Que.Select(x => new VMVoucherList()
            {
                VoucherNo = x.vou02full_no,
                VoucherType = x.vou01voucher_types.vou01title,
                ValueDate = x.vou02value_date,
                ManualVno = x.vou02manual_vno,
                Remarks = x.vou02description,
                TotalCredit = x.vou03voucher_details.Sum(d => d.vou03cr),
                TotalDebit = x.vou03voucher_details.Sum(d => d.vou03dr),
                Status = x.vou02status.ToString(),
                UpdatedName = "Admin"

            })
              .ToListAsync();

            return Ok(Result);
        }

        [HttpGet]
        [Route("GetExpenseVoucherList")]
        public async Task<IActionResult> GetExpenseVoucherList()
        {
            IQueryable<vou02voucher_summary> _Que = _context
                    .vou02voucher_summary
                    .OrderByDescending(x => x.DateCreated)
                    .Where(x => x.vou01voucher_types.vou01title == "Expense");
            if (_Que == null)
            {
                return NotFound("No Journal vouchers found.");
            }

            IList<VMVoucherList> Result = await _Que.Select(x => new VMVoucherList()
            {
                VoucherNo = x.vou02full_no,
                VoucherType = x.vou01voucher_types.vou01title,
                ValueDate = x.vou02value_date,
                ManualVno = x.vou02manual_vno,
                Remarks = x.vou02description,
                TotalCredit = x.vou03voucher_details.Sum(d => d.vou03cr),
                TotalDebit = x.vou03voucher_details.Sum(d => d.vou03dr),
                Status = x.vou02status.ToString(),
                UpdatedName = "Admin"

            })
              .ToListAsync();

            return Ok(Result);
        }



        [HttpGet]
        [Route("GetVoucherDetail/{id}")]
        public async Task<IActionResult> GetVoucherDetail(string id)
        {

            var voucherEntity = _context.vou02voucher_summary
                 .Include(v => v.vou01voucher_types)
                 .Include(x => x.led01ledgers)
                .FirstOrDefault(v => v.vou02full_no == id);

            if (voucherEntity == null)
            {
                return NotFound("voucher not found");
            }

            VMVoucherListDetail Result = new VMVoucherListDetail()
            {
                ID = voucherEntity.vou02full_no,
                VoucherNo = voucherEntity.vou02full_no,
                VoucherType = voucherEntity.vou01voucher_types.vou01title,
                ValueDate = voucherEntity.vou02value_date,
                ManualVno = voucherEntity.vou02manual_vno,
                Remarks = voucherEntity.vou02description,
                Status = voucherEntity.vou02status.ToString(),
                ContraLedgerId = voucherEntity.vou02contra_led05uin,
                UpdatedName = "Admin"
            };

            IQueryable<vou03voucher_details> _Que = _context
                  .vou03voucher_details
                  .Where(x => x.vou03vou02full_no == id);

            Result.VMDetails = await _Que.Select(x => new VMDetails()
            {
                ID = x.vou03uin,
                LedgerId = x.vou03led05uin,
                LedgerName = x.led01ledgers.led01title,
                LedgerNameCode = x.led01ledgers.led01code,
                Debit = x.vou03dr,
                Credit = x.vou03cr,
                Balance = x.vou03balance,
                Description = x.vou03description,
                ChqNo = x.vou03chq
            })
                  .ToListAsync();

            return Ok(Result);
        }


        [HttpGet]
        [Route("GetJournalVoucherDetail/{id}")]
        public async Task<IActionResult> GetJournalVoucherDetail(string id)
        {
            var voucherEntity = _context.vou02voucher_summary
                .Include(v => v.vou01voucher_types)
                .FirstOrDefault(v => v.vou02full_no == id && v.vou01voucher_types.vou01title == "Journal");


            if (voucherEntity == null)
            {
                return NotFound("voucher not found");
            }


            VMVoucherListDetail Result = new VMVoucherListDetail()
            {
                ID = voucherEntity.vou02full_no,
                VoucherNo = voucherEntity.vou02full_no,
                VoucherType = voucherEntity.vou01voucher_types.vou01title,
                ValueDate = voucherEntity.vou02value_date,
                ManualVno = voucherEntity.vou02manual_vno,
                Remarks = voucherEntity.vou02description,
                Status = voucherEntity.vou02status.ToString(),
                ContraLedgerId = voucherEntity.vou02contra_led05uin,
                UpdatedName = "Admin"
            };

            IQueryable<vou03voucher_details> _Que = _context
                  .vou03voucher_details
                  .Where(x => x.vou03vou02full_no == id);

            Result.VMDetails = await _Que.Select(x => new VMDetails()
            {
                ID = x.vou03uin,
                LedgerId = x.vou03led05uin,
                Debit = x.vou03dr,
                Credit = x.vou03cr,
                Balance = x.vou03balance,
                Description = x.vou03description,
                ChqNo = x.vou03chq
            })
                  .ToListAsync();

            return Ok(Result);
        }

        [HttpGet]
        [Route("GetIncomeVoucherDetail/{id}")]
        public async Task<IActionResult> GetIncomeVoucherDetail(string id)
        {
            var voucherEntity = _context.vou02voucher_summary
                 .Include(v => v.vou01voucher_types)
                 .FirstOrDefault(v => v.vou02full_no == id && v.vou01voucher_types.vou01title == "Income");


            if (voucherEntity == null)
            {
                return NotFound("voucher not found");
            }


            VMVoucherListDetail Result = new VMVoucherListDetail()
            {
                ID = voucherEntity.vou02full_no,
                VoucherNo = voucherEntity.vou02full_no,
                VoucherType = voucherEntity.vou01voucher_types.vou01title,
                ValueDate = voucherEntity.vou02value_date,
                ManualVno = voucherEntity.vou02manual_vno,
                Remarks = voucherEntity.vou02description,
                Status = voucherEntity.vou02status.ToString(),
                ContraLedgerId = voucherEntity.vou02contra_led05uin,
                UpdatedName = "Admin"
            };

            IQueryable<vou03voucher_details> _Que = _context
                  .vou03voucher_details
                  .Where(x => x.vou03vou02full_no == id);

            Result.VMDetails = await _Que.Select(x => new VMDetails()
            {
                ID = x.vou03uin,
                LedgerId = x.vou03led05uin,
                Debit = x.vou03dr,
                Credit = x.vou03cr,
                Balance = x.vou03balance,
                Description = x.vou03description,
                ChqNo = x.vou03chq
            })
                  .ToListAsync();

            return Ok(Result);
        }

        [HttpGet]
        [Route("GetExpenseVoucherDetail/{id}")]
        public async Task<IActionResult> GetExpenseVoucherDetail(string id)
        {
            var voucherEntity = _context.vou02voucher_summary
                .Include(v => v.vou01voucher_types)
                .FirstOrDefault(v => v.vou02full_no == id && v.vou01voucher_types.vou01title == "Expense");


            if (voucherEntity == null)
            {
                return NotFound("voucher not found");
            }


            VMVoucherListDetail Result = new VMVoucherListDetail()
            {
                ID = voucherEntity.vou02full_no,
                VoucherNo = voucherEntity.vou02full_no,
                VoucherType = voucherEntity.vou01voucher_types.vou01title,
                ValueDate = voucherEntity.vou02value_date,
                ManualVno = voucherEntity.vou02manual_vno,
                Remarks = voucherEntity.vou02description,
                Status = voucherEntity.vou02status.ToString(),
                ContraLedgerId = voucherEntity.vou02contra_led05uin,
                UpdatedName = "Admin"
            };

            IQueryable<vou03voucher_details> _Que = _context
                  .vou03voucher_details
                  .Where(x => x.vou03vou02full_no == id);

            Result.VMDetails = await _Que.Select(x => new VMDetails()
            {
                ID = x.vou03uin,
                LedgerId = x.vou03led05uin,
                Debit = x.vou03dr,
                Credit = x.vou03cr,
                Balance = x.vou03balance,
                Description = x.vou03description,
                ChqNo = x.vou03chq
            })
                  .ToListAsync();

            return Ok(Result);
        }


        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update(string id, VMVoucher updatedVoucher)
        {
            try
            {
                var voucherEntity = _context.vou02voucher_summary
                    .Include(v => v.vou03voucher_details)
                    .FirstOrDefault(v => v.vou02full_no == id);

                if (voucherEntity == null)
                {
                    return NotFound("Voucher not found.");
                }

                voucherEntity.vou02amount = updatedVoucher.Amount;
                voucherEntity.vou02description = updatedVoucher.Remarks;
                voucherEntity.vou02manual_vno = updatedVoucher.ManualVno;
                voucherEntity.vou02value_date = updatedVoucher.ValueDate;
                voucherEntity.vou02contra_led05uin = updatedVoucher.ContraLedgerId;

                if (updatedVoucher.VMVoucherDetailCreate != null)
                {
                    foreach (var updatedVoucherDetail in updatedVoucher.VMVoucherDetailCreate)
                    {
                        var ledger = _context.led01ledgers.FirstOrDefault(l => l.led01uin == updatedVoucherDetail.LedgerId);

                        if (ledger == null)
                        {
                            return BadRequest("Ledger not found.");
                        }

                        var existingDetail = voucherEntity.vou03voucher_details
                            .FirstOrDefault(d => d.vou03uin == updatedVoucherDetail.LedgerId);

                        if (existingDetail != null)
                        {
                            existingDetail.vou03led05uin = ledger.led01uin;
                            existingDetail.vou03dr = updatedVoucherDetail.Debit;
                            existingDetail.vou03cr = updatedVoucherDetail.Credit;
                            existingDetail.vou03description = updatedVoucherDetail.Description;
                            existingDetail.vou03chq = updatedVoucherDetail.ChqNo;
                            existingDetail.vou03balance = updatedVoucherDetail.Balance;
                        }
                    }

                    // Remove any details that are no longer present in the updated data
                    var detailsToRemove = voucherEntity.vou03voucher_details
                        .Where(d => !updatedVoucher.VMVoucherDetailCreate.Any(ud => ud.LedgerId == d.vou03uin))
                        .ToList();

                    foreach (var detailToRemove in detailsToRemove)
                    {
                        _context.vou03voucher_details.Remove(detailToRemove);
                    }
                }

                _context.SaveChanges();

                return Ok("Voucher updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            var voucherEntity = _context.vou02voucher_summary.FirstOrDefault(v => v.vou02full_no == id);

            if (voucherEntity == null)
            {
                return NotFound("Voucher not found.");
            }

            var voucherDetailsToDelete = _context.vou03voucher_details
             .Where(d => d.vou03vou02full_no == id)
            .ToList();

            foreach (var voucherDetail in voucherDetailsToDelete)
            {
                _context.vou03voucher_details.Remove(voucherDetail);
            }

            // Delete the parent voucher summary record
            var voucherSummaryToDelete = _context.vou02voucher_summary
                .FirstOrDefault(v => v.vou02full_no == id);

            if (voucherSummaryToDelete != null)
            {
                _context.vou02voucher_summary.Remove(voucherSummaryToDelete);
            }

            _context.SaveChanges();

            return Ok("Voucher deleted successfully.");
        }

        [HttpPatch]
        [Route("[action]/{id}")]
        public IActionResult Approve(string id, [FromBody] ChangeStatusRequest request)
        {

            return ChangeVoucherStatus(EnumVoucherStatus.Approved, id, request);
        }
        [HttpPatch]
        [Route("[action]/{id}")]
        public IActionResult Reject(string id, [FromBody] ChangeStatusRequest request)
        {
            return ChangeVoucherStatus(EnumVoucherStatus.Rejected, id, request);
        }
        [HttpPatch]
        [Route("[action]/{id}")]
        public IActionResult UnApprove(string id, [FromBody] ChangeStatusRequest request)
        {
            return ChangeVoucherStatus(EnumVoucherStatus.Pending, id, request);
        }

        private IActionResult ChangeVoucherStatus(EnumVoucherStatus status, string id, [FromBody] ChangeStatusRequest request)
        {
            try
            {
                var voucherEntity = _context.vou02voucher_summary.FirstOrDefault(v => v.vou02full_no == id);

                if (voucherEntity == null)
                {
                    throw new Exception("Voucher ID not found.");
                }
                EnumVoucherStatus oldStatus = voucherEntity.vou02status;
                //validation 
                //already approved cha ki nai/

                voucherEntity.vou02status = status;
                voucherEntity.vou02description = request.Remarks;
                _context.SaveChanges();

                //lets update ledger balance
                // Raise events based on status change
                if (oldStatus == EnumVoucherStatus.Pending && status == EnumVoucherStatus.Approved)
                {
                    VoucherApproved?.Invoke(voucherEntity);
                }
                if (status == EnumVoucherStatus.Pending && oldStatus == EnumVoucherStatus.Approved)
                {
                    VoucherUnApproved?.Invoke(voucherEntity);
                }

                return Ok("Done");
            }
            catch (Exception ex)
            {

                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        protected bool UpdateLedgerBalance(vou02voucher_summary Data, bool IsReverse = false)
        {
            foreach (var item in Data.vou03voucher_details)
            {
                if (item.led01ledgers.led05ledger_types.led05add_dr)
                {
                    item.led01ledgers.led01balance += (item.vou03dr - item.vou03cr) * (IsReverse ? -1 : 1);
                }
                else
                {
                    item.led01ledgers.led01balance += (item.vou03cr - item.vou03dr) * (IsReverse ? -1 : 1);
                }
                _context.Entry(item).State = EntityState.Modified;
            }
            _context.SaveChanges();
            return true;
        }

        protected bool OnVoucherApproved(vou02voucher_summary Data)
        {
            ///update related sub ledgers ko data to their balance
            return UpdateLedgerBalance(Data, false);
        }
        protected bool OnVoucherRejected(vou02voucher_summary Data)
        {
            return true;//
        }
        protected bool OnVoucherUnApproved(vou02voucher_summary Data)
        {
            ///update related sub ledgers ko data to their balance
            return UpdateLedgerBalance(Data, true);
        }
    }
}
