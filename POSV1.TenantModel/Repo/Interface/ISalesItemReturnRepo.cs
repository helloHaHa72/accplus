using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface ISalesItemReturnRepo : RepoBaseModelCore.IGeneralRepositories<sal02itemsreturn, int>
    {
        void LoadSaleReturnItemDetails(sal01salesreturn sal01Sales);
    }
}
