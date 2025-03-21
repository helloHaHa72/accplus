using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Production
{
    public class ProductionRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, prod01production, int>, IProductionRepo
    {
        public ProductionRepo(MainDbContext context) : base(context)
        {
            
        }

        public override IQueryable<prod01production> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
