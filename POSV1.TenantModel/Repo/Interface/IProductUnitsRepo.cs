using POSV1.TenantModel.Models.EntityModels.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface IProductUnitsRepo : RepoBaseModelCore.IGeneralRepositories<pro03units, int>
    {
        Task RemoveRangeAsync(IEnumerable<pro03units> entities);
    }
}
