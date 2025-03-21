using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface IPurchaseRepo : RepoBaseModelCore.IGeneralRepositories<pur01purchases, int>
    {
        Task UpdateRangeAsync(IEnumerable<pur01purchases> purchase);
    }
}
