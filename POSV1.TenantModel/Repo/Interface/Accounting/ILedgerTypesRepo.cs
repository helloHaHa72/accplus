using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Accounting;

namespace POSV1.TenantModel.Repo.Interface.Accounting
{
    public interface ILedgerTypesRepo:  RepoBaseModelCore.IGeneralRepositories<led05ledger_types, int>
    {
        
    }
}
