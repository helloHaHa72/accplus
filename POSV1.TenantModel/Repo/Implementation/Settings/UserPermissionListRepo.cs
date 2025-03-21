using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantModel.Repo.Interface.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Settings
{
    public class UserPermissionListRepo
        : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, UserPermissionList, int>,
        IUserPermissionListRepo
    {
        public UserPermissionListRepo(MainDbContext context) : base(context)
        {
        }

        public override IQueryable<UserPermissionList> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
