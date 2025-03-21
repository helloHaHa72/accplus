using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Accounting;

namespace POSV1.TenantModel.Repo.Interface.Accounting
{
    public interface IGLedgersRepo: RepoBaseModelCore.IGeneralRepositories<led03general_ledgers, int>
    {
        Task<led03general_ledgers> GetGeneralLedgerByUinAsync(int uin);
    }
}
