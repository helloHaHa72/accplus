using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface.Accounting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayBookController : ControllerBase
    {
        private readonly IItemTransactionService _itemTransactionService;
        private readonly ILedgersRepo _ledgersRepo;
        private readonly IVoucherDetailsRepo _voucherDetailRepo;

        public DayBookController(
            IItemTransactionService itemTransactionService, 
            ILedgersRepo ledgersRepo,
            IVoucherDetailsRepo voucherDetailsRepo)
        {
            _itemTransactionService = itemTransactionService;
            _ledgersRepo = ledgersRepo;
            _voucherDetailRepo = voucherDetailsRepo;
        }

        [HttpGet("GetTransactionSummary")]
        public async Task<IActionResult> GetTransactionSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var summary = await _itemTransactionService.GetTransactionSummary(startDate, endDate);

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetExpensesSummary/LedgerWise")]
        public async Task<IActionResult> GetExpensesSummary([FromQuery] DateTime? Date)
        {
            try
            {
                // Fetch all ledgers with the type "Expenses"
                var ledgers = await _ledgersRepo.GetList()
                    .Include(x => x.led05ledger_types)
                    .Include(x => x.vou02voucher_summary)
                    .Where(vd => vd.led05ledger_types.led05title == "Expenses")
                    .GroupBy(x => x.led01title)
                    .Select(g => new ExpenseSummaryDTO
                    {
                        LedgerName = g.Key,
                        Balance = g.Sum(x => x.led01balance) // Sum led01balance irrespective of date
                    })
                    .OrderBy(x => x.LedgerName)
                    .ToListAsync();

                var expenseSummaryDetail = new ExpenseSummaryDetail
                {
                    Date = Date,
                    Particulars = $"Expense summary of {Date?.ToString("yyyy-MM-dd") ?? "all time"}",
                    Ledgers = ledgers
                };

                return Ok(expenseSummaryDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetExpensesSummary")]
        public async Task<IActionResult> GetDayExpensesSummary([FromQuery] DateTime? Date)
        {
            try
            {
                var ledgers = await _ledgersRepo.GetList()
                    .Include(x => x.led05ledger_types)
                    .Where(vd => vd.led05ledger_types.led05title == "Expenses")
                    .Select(x => new { x.led01uin, x.led01title })
                    .ToListAsync();

                var ledgerIds = ledgers.Select(l => l.led01uin).ToList();

                var voucherDetails = await _voucherDetailRepo.GetList()
                    .Where(x => ledgerIds.Contains(x.vou03led05uin) && x.DateCreated.Date == Date)
                    .GroupBy(x => x.vou03led05uin)
                    .Select(g => new { LedgerId = g.Key, Balance = g.Sum(v => v.vou03balance) })
                    .ToListAsync();

                var expenseSummaryList = ledgers
                    .Select(ledger => new ExpenseSummaryDTO
                    {
                        LedgerName = ledger.led01title,
                        Balance = voucherDetails.FirstOrDefault(v => v.LedgerId == ledger.led01uin)?.Balance ?? 0
                    })
                    .GroupBy(x => x.LedgerName) // Grouping by LedgerName
                    .Select(g => new ExpenseSummaryDTO
                    {
                        LedgerName = g.Key,
                        Balance = g.Sum(x => x.Balance) // Summing the balances for the same ledger
                    })
                    .OrderBy(x => x.LedgerName) // Ordering alphabetically
                    .ToList();

                return Ok(new ExpenseSummaryDetail
                {
                    Date = Date,
                    Particulars = $"Expense summary of {Date?.ToString("yyyy-MM-dd") ?? "all time"}",
                    Ledgers = expenseSummaryList
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}
