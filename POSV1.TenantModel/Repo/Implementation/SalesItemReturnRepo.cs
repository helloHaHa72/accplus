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
    internal class SalesItemReturnRepo :
    RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, sal02itemsreturn, int>,
    ISalesItemReturnRepo
    {
        private readonly MainDbContext _context;
        public SalesItemReturnRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }


        public override IQueryable<sal02itemsreturn> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public void LoadSaleReturnItemDetails(sal01salesreturn sal01Sales)
        {
            _context.Entry(sal01Sales)
                       .Collection(t => t.sal02itemsreturn)
                       .Load();
        }
    }
}
