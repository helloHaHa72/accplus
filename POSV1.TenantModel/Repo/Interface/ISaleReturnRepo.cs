using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface ISaleReturnRepo : RepoBaseModelCore.IGeneralRepositories<sal01salesreturn, int>
    {
        Task UpdateRangeAsync(IEnumerable<sal01salesreturn> salesret);
        IQueryable<object> GetSalesReturnList(int emp_id);
    }
}
