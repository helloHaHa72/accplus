using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class ProductUnitsRepo :
    RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, pro03units, int>,
    IProductUnitsRepo
    {
        private readonly MainDbContext _context;
        public ProductUnitsRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<pro03units> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public async Task RemoveRangeAsync(IEnumerable<pro03units> entities)
        {
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync(); // Automatically save after removing
        }

        //public override IQueryable<pro03units> GetList(bool IncludeDeleted = false)
        //{
        //    var query = base.GetList(IncludeDeleted);

        //    query = query.Where(c => c.pro03status);
        //    //if (!IncludeDeleted)
        //    //{
        //    //    query = query.Where(c => c.pro01status);
        //    //}

        //    return query;
        //}

        //public void DeleteOldUnits(int productId)
        //{
        //    var existingUnits = context.Set<pro03units>()
        //        .Where(x => x.pro03pro02uin == productId)
        //        .ToList();

        //    if (existingUnits.Any())
        //    {
        //        context.Set<pro03units>().RemoveRange(existingUnits);
        //        context.SaveChanges();  // Persist changes in the database
        //    }
        //}
    }
}
