using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class ConfigRepo :
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, Config, int>,
        IConfigRepo
    {
        public ConfigRepo(MainDbContext context) : base(context)
        {

        }
    }
}
