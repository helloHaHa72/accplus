using System;
using POSV1.TenantModel.Models;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface IVoucherDetailsRepo : RepoBaseModelCore.IGeneralRepositories<vou03voucher_details, int>
    {
        void LoadVoucherDetails(vou02voucher_summary voucherSummary);
    }
}
