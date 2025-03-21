using System.Linq;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class ProductsRepo :
    RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, pro02products, int>,
    IProductsRepo
    {
        public ProductsRepo(MainDbContext context) : base(context)
        {

        }

        public override IQueryable<pro02products> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
