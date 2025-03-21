using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantModel.Repo.Interface.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Settings
{
    public class AccessListRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, AccessList, int>, IAccessListRepo
    {
        public AccessListRepo(MainDbContext context) : base(context)
        {   
        }
    }
}
