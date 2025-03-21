using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class PurchaseReturnRepo :
            RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, pur01purchasereturns, int>,
    IPurchaseReturnRepo
    {
        private readonly MainDbContext _context;
        public PurchaseReturnRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<pur01purchasereturns> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public async Task UpdateRangeAsync(IEnumerable<pur01purchasereturns> purchaseret)
        {
            _context.UpdateRange(purchaseret);
            await _context.SaveChangesAsync();
        }
    }
}
