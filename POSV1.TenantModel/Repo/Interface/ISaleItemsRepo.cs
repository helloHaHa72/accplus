using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface ISaleItemsRepo : RepoBaseModelCore.IGeneralRepositories<sal02items, int>
    {
        void LoadSaleItemDetails(sal01sales sal01Sales);
    }
}
