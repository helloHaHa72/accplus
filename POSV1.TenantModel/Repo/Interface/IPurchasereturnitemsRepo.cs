using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface IPurchasereturnitemsRepo : RepoBaseModelCore.IGeneralRepositories<pur02returnitems, int>
    {
        void LoadPurchaseReturnItemDetails(pur01purchasereturns pur01purchasereturns);
    }
}
