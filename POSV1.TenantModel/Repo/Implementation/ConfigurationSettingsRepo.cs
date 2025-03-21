using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Settings;
using POSV1.TenantModel.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo
{
    public class ConfigurationSettingsRepo :
            RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, ConfigurationSetting, int>,
    IConfigurationSettings
    {
        public ConfigurationSettingsRepo(MainDbContext context) : base(context)
        {

        }

        public override IQueryable<ConfigurationSetting> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue).OrderByDescending(x => x.DateCreated);
            return _Query;
        }
    }
}
