using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Production
{
    public class PurchaseAdditionalCharges : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, add02purchaseadditionalcharges, int>, IPurchaseAdditionalCharges
    {
        private readonly MainDbContext _context;
        public PurchaseAdditionalCharges(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<add02purchaseadditionalcharges> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
