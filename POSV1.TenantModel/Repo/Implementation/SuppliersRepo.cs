using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class SuppliersRepo:
         RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, sup01suppliers, int>,
        ISuppliersRepo
    {
        public SuppliersRepo(MainDbContext context) : base(context)
        {

        }


        public override IQueryable<sup01suppliers> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
