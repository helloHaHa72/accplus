using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class CashSettlementRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, cas01cashsettlement, int>, ICashSettlementRepo
    {
        private readonly MainDbContext _context;
        public CashSettlementRepo(MainDbContext context) : base(context)
        {   
            _context = context;
        }

        public override IQueryable<cas01cashsettlement> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue).OrderByDescending(x => x.DateCreated);
            return _Query;
        }

        public async Task UpdateRangeAsync(IEnumerable<cas01cashsettlement> cash)
        {
            _context.UpdateRange(cash);
            await _context.SaveChangesAsync();
        }

        public IQueryable<object> GetCashSettlementListCustomer(int emp_id)
        {
            return _context.cas01cashsettlement
                .Where(c => c.cas01customeruin == emp_id)
                .Select(c => new
                {
                    c.cas01payment_type,
                    c.cas01tdspercentage,
                    c.cas01amount,
                    c.cas01transaction_date
                });
        }
    }
}
