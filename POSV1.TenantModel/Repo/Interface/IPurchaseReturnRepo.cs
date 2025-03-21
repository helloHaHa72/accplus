using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface IPurchaseReturnRepo : RepoBaseModelCore.IGeneralRepositories<pur01purchasereturns, int>
    {
        Task UpdateRangeAsync(IEnumerable<pur01purchasereturns> purchaseret);
    }
}
