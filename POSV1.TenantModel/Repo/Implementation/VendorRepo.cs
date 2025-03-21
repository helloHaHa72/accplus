using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class VendorRepo:
            RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, ven01vendors, int>,
             IVendorRepo
    {
        public VendorRepo(MainDbContext context) : base(context)
        {

        }

        public override IQueryable<ven01vendors> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
