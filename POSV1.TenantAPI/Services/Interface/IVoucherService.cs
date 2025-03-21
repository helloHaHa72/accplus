using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantAPI.Services
{
    public interface IVoucherService
    {
        Task<bool> CreateVoucherLogOnStatusUpdate(EnumVoucherStatus status, string voucherId);
    }
}
