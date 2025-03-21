using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class VoucherSummaryRepo:
          RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, vou02voucher_summary, string>,
         IVoucherSummaryRepo
    {
        public VoucherSummaryRepo(MainDbContext context) : base(context)
        {
            
        }

        public override IQueryable<vou02voucher_summary> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
