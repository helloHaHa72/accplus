using AutoMapper;
using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : 
          _AbsCRUDWithDiffInputModelController<ConfigController, IConfigRepo, Config, VMConfig, VMConfigList, VMConfigList, int>
    {
        private readonly IMapper _mapper;
        public ConfigController(
            ILogger<ConfigController> logger,
            IMapper mapper,
            IConfigRepo configRepo
            )
           : base(logger, configRepo, mapper)
        {
            _mapper = mapper;
        }
        protected override IQueryable<VMConfigList> ProcessListData(IQueryable<Config> data)
        {
            return data.Select(config => new VMConfigList
            {
                id = config.id,
                key = config.key.ToString(),
                value = config.value,
            });
        }

        protected override VMConfigList ProcessDetailData(Config data)
        {
            return new VMConfigList
            {
                id = data.id,
                key = data.key.ToString(),
                value = data.value,
            };
        }

        protected override Config AssignValues(VMConfig Data)
        {
            try
            {
                var configEntity = new Config
                {
                    key = (enumConfigSettingsKeys)Data.key,
                    value = Data.value,

                    CreatedName = _ActiveUserName,
                    DateCreated = DateTime.Now,
                    UpdatedName = " ",
                    DateUpdated = DateTime.Now,
                    DeletedName = "",
                };
                return configEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void ReAssignValues(VMConfig Data, Config oldData)
        {
            oldData.key = (enumConfigSettingsKeys)Data.key;
            oldData.value = Data.value;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;
        }

    }
}
