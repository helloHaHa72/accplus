using POSV1.MasterDBModel.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface.Settings
{
    public interface IAccessListRepo : RepoBaseModelCore.IGeneralRepositories<AccessList, int>
    {
    }
}
