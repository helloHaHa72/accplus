using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface IConfigurationSettings : RepoBaseModelCore.IGeneralRepositories<ConfigurationSetting, int>
    {
    }
}
