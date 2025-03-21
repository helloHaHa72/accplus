using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface.ERP
{
    public interface ITaxSettlementRepo : RepoBaseModelCore.IGeneralRepositories<ta01taxsettlement, int>
    {
    }
}
