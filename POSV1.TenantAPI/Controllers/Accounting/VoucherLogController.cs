using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherLogController : ControllerBase
    {
        private readonly IVoucherLogRepo _voucherLogRepo;
        public VoucherLogController(IVoucherLogRepo voucherLogRepo)
        {
            _voucherLogRepo = voucherLogRepo;
        }

        [HttpGet]
        [Route("GetVoucherLog/{id}")]
        public async Task<IActionResult> GetVoucherLog(string id)
        {
            var voucherLogEntity = _voucherLogRepo.GetList().OrderByDescending(x => x.DateCreated).Where(x => x.vou05vou02uin == id);

            var result = voucherLogEntity.Select(x => new VMVoucherLog
            {
                Id = x.vou05uin,
                VoucherId = x.vou05vou02uin,
                Status = x.voucherStatus.ToString(),
                _createdDate = x.DateCreated,
                CreatedBy = x.CreatedName
            });

            return Ok(result);
        }
    }
}
