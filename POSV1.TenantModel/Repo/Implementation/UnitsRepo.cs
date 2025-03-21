using System.Linq;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class UnitsRepo :
   RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, un01units, int>,
   IUnitsRepo
    {
        public UnitsRepo(MainDbContext context) : base(context)
        {
        }
        //public override IQueryable<un01units> FilterActive()
        //{
        //    _Query = base.FilterActive().Where(x => x.un01status);
        //    return _Query;
        //}
        public override IQueryable<un01units> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
