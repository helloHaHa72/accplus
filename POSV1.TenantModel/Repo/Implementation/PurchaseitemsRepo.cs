using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class PurchaseitemsRepo:
            RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, pur02items, int>,
    IPurchaseItemsRepo
    {
        private readonly MainDbContext _context;
        public PurchaseitemsRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }
        public override IQueryable<pur02items> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public void LoadPurchaseItemDetails(pur01purchases pur01Purchases)
        {
            _context.Entry(pur01Purchases)
                       .Collection(t => t.pur02items)
                       .Load();
        }
    }
}
