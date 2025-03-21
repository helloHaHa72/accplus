using POSV1.TenantModel.Models.EntityModels.Estimate;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface.Estimate
{
    public interface IEstimateRepo : RepoBaseModelCore.IGeneralRepositories<est01estimate, int>
    {
    }
}
