using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface ICashSettlementRepo : RepoBaseModelCore.IGeneralRepositories<cas01cashsettlement, int>
    {
        Task UpdateRangeAsync(IEnumerable<cas01cashsettlement> cash);
        IQueryable<object> GetCashSettlementListCustomer(int emp_id);
    }
}
