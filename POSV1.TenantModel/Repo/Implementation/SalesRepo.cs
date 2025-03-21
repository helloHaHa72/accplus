using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    internal class SalesRepo :
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, sal01sales, int>,
        ISalesRepo
    {

        private readonly MainDbContext _context;
        public SalesRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<sal01sales> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue).OrderByDescending(x => x.DateCreated);
            return _Query;
        }

        public async Task InsertBulkAsync(List<sal01sales> entity)
        {
           await  _context.AddRangeAsync(entity);
           await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<sal01sales> sales)
        {
            _context.UpdateRange(sales);
            await _context.SaveChangesAsync();
        }

    }
}
