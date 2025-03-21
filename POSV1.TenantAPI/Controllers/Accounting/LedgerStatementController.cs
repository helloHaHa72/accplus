using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel;
using POSV1.TenantAPI.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using POSV1.TenantModel.Models;
using Microsoft.AspNetCore.Authorization;
using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace POSV1.TenantAPI.Controllers.Accounting
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedgerStatementController : ControllerBase
    {
        private readonly MainDbContext _context;

        public LedgerStatementController(MainDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ReqLedgerStatement ledgerStatement)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,

                };

                var LedgerDetail = _context.led01ledgers.FirstOrDefault(pl => pl.led01uin == ledgerStatement.LedgerID);
                if (LedgerDetail == null)
                {
                    return BadRequest("Invalid ID");
                }

                var ParentGL = _context.led03general_ledgers.FirstOrDefault(x => x.led03uin == LedgerDetail.led01led03uin);
                if (ParentGL == null)
                {
                    return BadRequest("Invalid ID");
                }

                VMLedgerStatement Result = new VMLedgerStatement()
                {
                    LedgerID = LedgerDetail.led01uin,
                    LedgerName = LedgerDetail.led01title,
                    ParentGLID = LedgerDetail.led01led03uin,
                    ParentGLName = ParentGL.led03title
                };
                IQueryable<vou03voucher_details> _Que = _context
                    .vou03voucher_details
                    .Where(vd =>
                        vd.vou03led05uin == ledgerStatement.LedgerID);

                if (ledgerStatement.FromDate.HasValue)
                {
                    _Que = _Que.Where(x => x.vou02voucher_summary.vou02value_date >= ledgerStatement.FromDate.Value);
                }
                if (ledgerStatement.ToDate.HasValue)
                {
                    _Que = _Que.Where(x => x.vou02voucher_summary.vou02value_date <= ledgerStatement.ToDate.Value);
                }

                _Que = _Que.Where(x => x.vou02voucher_summary.vou02status == EnumVoucherStatus.Approved);

                Result.VMLedgerStmtDetail = await _Que.Select(x => new VMLedgerStmtDetail()
                {
                    ID = x.vou03uin,
                    VoucherNo = x.vou03vou02full_no,
                    ValueDate = x.vou02voucher_summary.vou02value_date,
                    Narration = x.vou03description,
                    Debit = x.vou03dr,
                    Credit = x.vou03cr,
                    Balance = x.vou03balance,
                    UpdatedName = x.UpdatedName

                })
                .ToListAsync();

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
