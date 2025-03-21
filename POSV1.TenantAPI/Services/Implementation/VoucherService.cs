using System.Security.Claims;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IVoucherLogRepo _voucherLogRepo;
        public VoucherService(IHttpContextAccessor contextAccessor , IVoucherLogRepo voucherLogRepo)
        {
            _contextAccessor = contextAccessor;
            _voucherLogRepo = voucherLogRepo;
        }
        public async Task<bool> CreateVoucherLogOnStatusUpdate(EnumVoucherStatus status, string voucherId)
        {
            try
            {
                var user = _contextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;

                var voucherLogEntity = new vou05voucher_log
                {
                    vou05vou02uin = voucherId,
                    voucherStatus = status,
                    DateCreated = DateTime.UtcNow,
                    CreatedName = user
                };

                _voucherLogRepo.Insert(voucherLogEntity);
                _voucherLogRepo.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
