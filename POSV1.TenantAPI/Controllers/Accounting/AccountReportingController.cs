using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantAPI.Controllers.Accounting
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountReportingController : ControllerBase
    {
        private MainDbContext _context;
        private readonly ILedgersRepo _ledgersRepo;
        private readonly IVoucherDetailsRepo _voucherDetailsRepo;
        public AccountReportingController(
            MainDbContext context,
            ILedgersRepo ledgersRepo,
            IVoucherDetailsRepo voucherDetailsRepo)
        {
            _context = context;
            _ledgersRepo = ledgersRepo;
            _voucherDetailsRepo = voucherDetailsRepo;
        }

        //[HttpGet("GetTrialBalanceList")]
        //public async Task<IActionResult> GetTrialBalanceList()
        //{
        //    try
        //    {
        //        VMAccReport Result = new VMAccReport();

        //        IQueryable<led01ledgers> _Que = _ledgersRepo.GetList()
        //            .Include(x => x.led05ledger_types)
        //            .Where(vd => vd.led01balance != 0)
        //            .OrderByDescending(x => x.DateCreated);

        //        Result.AccReportDetail = await _Que.Select(x => new VMAccReportDetail()
        //        {
        //            LedgerId = x.led01uin,
        //            TypeId = (EnumLedgerTypes)x.led01led05uin,
        //            LedgerType = x.led05ledger_types.led05title,
        //            LedgerName = x.led01title,
        //            LedgerCode = x.led01code,
        //            Balance = x.led01balance,
        //        })
        //        .ToListAsync();

        //        return Ok(Result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //    }
        //}


        [HttpGet("GetTrialBalanceList")]
        public async Task<IActionResult> GetTrialBalanceList()
        {
            try
            {
                VMAccReport Result = new VMAccReport();

                IQueryable<led01ledgers> _Que = _ledgersRepo.GetList()
                    .Include(x => x.led05ledger_types)
                    .Where(vd => vd.led01balance != 0)
                    .OrderByDescending(x => x.DateCreated);

                // Fetching the data and sorting by Ledger Type, then LedgerName within each type
                var sortedDetails = await _Que
                    .Select(x => new VMAccReportDetail()
                    {
                        LedgerId = x.led01uin,
                        TypeId = (EnumLedgerTypes)x.led01led05uin,
                        LedgerType = x.led05ledger_types.led05title,
                        LedgerName = x.led01title,
                        LedgerCode = x.led01code,
                        Balance = x.led01balance,
                    })
                    .ToListAsync();

                // Now sorting the data dynamically based on ledger type, then by LedgerName within each type
                var orderedDetails = sortedDetails
                    .OrderBy(x => GetLedgerTypeOrder(x.TypeId)) // Order first by Ledger Type (Liabilities, Assets, Income, Expenses)
                    .ThenBy(x => x.LedgerType == "Liabilities" ? x.LedgerName : "")
                    .ThenBy(x => x.LedgerType == "Assets" ? x.LedgerName : "")
                    .ThenBy(x => x.LedgerType == "Income" ? x.LedgerName : "")
                    .ThenBy(x => x.LedgerType == "Expenses" ? x.LedgerName : "")
                    .ToList();

                Result.AccReportDetail = orderedDetails;

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Helper method to map EnumLedgerTypes to order values for sorting
        private int GetLedgerTypeOrder(EnumLedgerTypes type)
        {
            switch (type)
            {
                case EnumLedgerTypes.Liabilities:
                    return 1;
                case EnumLedgerTypes.Assets:
                    return 2;
                case EnumLedgerTypes.Income:
                    return 3;
                case EnumLedgerTypes.Expenses:
                    return 4;
                default:
                    return 5; // Default case in case there is an unknown ledger type
            }
        }


        //liability and assets  
        [HttpGet("GetBalanceSheetList")]
        public async Task<IActionResult> GetBalanceSheetList()
        {
            IQueryable<led01ledgers> _Que = _ledgersRepo
                .GetList()
                .Include(x => x.led05ledger_types)
                .OrderBy(x => x.led01title)
                .Where(pl => pl.led01led05uin == (int)EnumGLType.Liabilities || pl.led01led05uin == (int)EnumGLType.Assets && pl.led01balance != 0);

            VMAccReport Result = new VMAccReport();

            Result.AccReportDetail = await _Que.Select(x => new VMAccReportDetail()
            {
                LedgerId = x.led01uin,
                TypeId = (EnumLedgerTypes)x.led01led05uin,
                LedgerType = x.led05ledger_types.led05title,
                LedgerName = x.led01title,
                LedgerCode = x.led01code,
                Balance = x.led01balance,
            })
            .ToListAsync();

            return Ok(Result);
        }

        //[HttpGet("GetPLSheetList")]
        //public async Task<IActionResult> GetPLSheetList()
        //{
        //    IQueryable<led01ledgers> _Que = _ledgersRepo.GetList()
        //        .Include(x => x.led05ledger_types)
        //        .Where(pl => pl.led01led05uin == (int)EnumGLType.Income || pl.led01led05uin == (int)EnumGLType.Expenses);

        //    VMAccReport Result = new VMAccReport();

        //    _Que = _Que.Where(vd => vd.led01balance != 0);
        //    //&& vd.led01ledgers.led01balance > 0 || vd.led01ledgers.led01balance < 0);

        //    Result.AccReportDetail = await _Que.Select(x => new VMAccReportDetail()
        //    {
        //        LedgerId = x.led01uin,
        //        TypeId = (EnumLedgerTypes)x.led01led05uin,
        //        LedgerType = x.led05ledger_types.led05title,
        //        LedgerName = x.led01title,
        //        LedgerCode = x.led01code,
        //        Balance = x.led01balance,
        //    })
        //    .ToListAsync();

        //    return Ok(Result);
        //}

        [HttpGet("GetPLSheetList")]
        public async Task<IActionResult> GetPLSheetList()
        {
            try
            {
                IQueryable<led01ledgers> _Que = _ledgersRepo.GetList()
                    .Include(x => x.led05ledger_types)
                    .Where(pl => pl.led01led05uin == (int)EnumGLType.Income || pl.led01led05uin == (int)EnumGLType.Expenses)
                    .Where(vd => vd.led01balance != 0);

                VMAccReport Result = new VMAccReport();

                var sortedDetails = await _Que
                    .Select(x => new VMAccReportDetail()
                    {
                        LedgerId = x.led01uin,
                        TypeId = (EnumLedgerTypes)x.led01led05uin,
                        LedgerType = x.led05ledger_types.led05title,
                        LedgerName = x.led01title,
                        LedgerCode = x.led01code,
                        Balance = x.led01balance,
                    })
                    .ToListAsync();

                var orderedDetails = sortedDetails
                    .OrderBy(x => GetPLTypeOrder(x.TypeId))
                    .ThenBy(x => x.LedgerName)
                    .ToList();

                Result.AccReportDetail = orderedDetails;

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private int GetPLTypeOrder(EnumLedgerTypes type)
        {
            switch (type)
            {
                case EnumLedgerTypes.Income:
                    return 1;
                case EnumLedgerTypes.Expenses:
                    return 2;
                default:
                    return 3;
            }
        }


    }
}
