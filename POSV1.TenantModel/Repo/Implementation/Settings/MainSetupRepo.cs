using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Settings;
using POSV1.TenantModel.Repo.Interface.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Settings
{
    public class MainSetupRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, MainSetup, int>, IMainSetupRepo
    {
        public MainSetupRepo(MainDbContext context) : base(context)
        {
            
        }

        public override IQueryable<MainSetup> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
