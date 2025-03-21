using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantModel.Repo.Implementation.Accounting
{
    public class LedgerTypesRepo:
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, led05ledger_types, int>,
         ILedgerTypesRepo
    {
        public LedgerTypesRepo(MainDbContext context) : base(context)
        {
            
        }

        //public override IQueryable<led05ledger_types> FilterActive()
        //{
        //    _Query = base.FilterActive().Where(x => x.led05add_dr);
        //    return _Query;
        //}
        public override IQueryable<led05ledger_types> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

       
    }
}
