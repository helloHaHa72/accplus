using System;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface IPurchaseItemsRepo : RepoBaseModelCore.IGeneralRepositories<pur02items, int>
    {
        void LoadPurchaseItemDetails(pur01purchases pur01Purchases);
    }
}
