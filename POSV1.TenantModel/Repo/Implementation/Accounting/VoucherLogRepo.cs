using System;
using System.Linq;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class VoucherLogRepo :
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, vou05voucher_log, string>, 
        IVoucherLogRepo
    {
        public VoucherLogRepo(MainDbContext context) : base(context)
        {

        }

        public override IQueryable<vou05voucher_log> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
