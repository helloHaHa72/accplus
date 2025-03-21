using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class PurchasereturnitemsRepo :
            RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, pur02returnitems, int>,
    IPurchasereturnitemsRepo
    {
        private readonly MainDbContext _context;
        public PurchasereturnitemsRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }
        public override IQueryable<pur02returnitems> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
        public void LoadPurchaseReturnItemDetails(pur01purchasereturns pur01purchasereturns)
        {
            _context.Entry(pur01purchasereturns)
                       .Collection(t => t.pur02returnitems)
                       .Load();
        }
    }
}
