using POSV1.TenantModel.Models.EntityModels.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface.Production
{
    public interface IConsumeRawProductRepo : RepoBaseModelCore.IGeneralRepositories<prod02consumerawproduct, int>
    {
        Task RemoveRangeAsync(IEnumerable<prod02consumerawproduct> products);
    }
}
