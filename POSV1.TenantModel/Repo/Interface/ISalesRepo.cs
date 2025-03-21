using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface ISalesRepo : RepoBaseModelCore.IGeneralRepositories<sal01sales, int>
    {

        Task UpdateRangeAsync(IEnumerable<sal01sales> sales);
        Task InsertBulkAsync(List<sal01sales> entity);
    }
}
