using System.Threading.Tasks;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Repo.Interface.Accounting
{
    public interface ILedgersRepo : RepoBaseModelCore.IGeneralRepositories<led01ledgers, int>
    {
        Task<led03general_ledgers> GetGeneralLedgerByUinAsync(int uin);

        Task<led05ledger_types> GetLedgerTypeByTitleAsync(string title);

 
    }
}
