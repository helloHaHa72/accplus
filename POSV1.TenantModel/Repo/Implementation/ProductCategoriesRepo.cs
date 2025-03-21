using System.Linq;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class ProductCategoriesRepo :
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, pro01categories, int>,
        IProductCategoriesRepo
    {
        public ProductCategoriesRepo(MainDbContext context) : base(context)
        {

        }

        public override IQueryable<pro01categories> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        //public override IQueryable<pro01categories> GetList(bool IncludeDeleted = false)
        //{
        //    var query = base.GetList(IncludeDeleted);

        //    query = query.Where(c => c.pro01status);
        //    //if (!IncludeDeleted)
        //    //{
        //    //    query = query.Where(c => c.pro01status);
        //    //}

        //    return query;
        //}
    }

}
