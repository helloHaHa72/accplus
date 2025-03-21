using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class PurchaseRepo:
            RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, pur01purchases, int>,
    IPurchaseRepo
    {
        private readonly MainDbContext _context;
        public PurchaseRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<pur01purchases> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue).OrderByDescending(x => x.DateCreated);
            return _Query;
        }

        public async Task UpdateRangeAsync(IEnumerable<pur01purchases> purchase)
        {
            _context.UpdateRange(purchase);
            await _context.SaveChangesAsync();
        }
    }
}
