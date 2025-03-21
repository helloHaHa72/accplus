using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class VoucherDetailsRepo :
          RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, vou03voucher_details, int>,
         IVoucherDetailsRepo
    {
        private MainDbContext _context;
        public VoucherDetailsRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }
     
        public void LoadVoucherDetails(vou02voucher_summary voucherSummary)
        {
            _context.Entry(voucherSummary)
                      .Collection(t => t.vou03voucher_details)
                      .Load();
        }

        public override IQueryable<vou03voucher_details> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
