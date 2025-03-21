using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo
{
    internal class SaleReturnRepo :
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, sal01salesreturn, int>,
        ISaleReturnRepo
    {

        private readonly MainDbContext _context;
        public SaleReturnRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<sal01salesreturn> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public async Task InsertBulkAsync(List<sal01salesreturn> entity)
        {
            await _context.AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<sal01salesreturn> salesret)
        {
            _context.UpdateRange(salesret);
            await _context.SaveChangesAsync();
        }

        public IQueryable<object> GetSalesReturnList(int emp_id)
        {
            return _context.sal01sales
                .Where(s => s.sal01cus01uin == emp_id)
                .Select(s => new
                {
                    s.sal01date_nep,
                    s.sal01disc_amt,
                    s.sal01vat_amt,
                    s.sal01net_amt,
                    s.sal01voucher_no
                });
        }
    }
}
