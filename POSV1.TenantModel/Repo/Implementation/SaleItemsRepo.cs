using System.Linq;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    internal class SaleItemsRepo :
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, sal02items, int>,
        ISaleItemsRepo
    {
        private readonly MainDbContext _context;
        public SaleItemsRepo(MainDbContext context): base(context) 
        {
            _context = context;
        }


        public override IQueryable<sal02items> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public void LoadSaleItemDetails(sal01sales sal01Sales)
        {
            _context.Entry(sal01Sales)
                       .Collection(t => t.sal02items)
                       .Load();
        }
    }
}
