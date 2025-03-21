using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SystemConfigurationController : ControllerBase
    {
        private readonly IConfigurationSettings _configurationSettings;
        private readonly ILogger<SystemConfigurationController> _logger;
        public SystemConfigurationController(
            IConfigurationSettings configurationSettings,
            ILogger<SystemConfigurationController> logger)
        {
            _configurationSettings = configurationSettings;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var configData = await _configurationSettings.GetList().ToListAsync();

                var configEntity = configData.Select(configuration => new VMSystemConfig
                {
                    Id = configuration.Id,
                    Name = configuration.Name,
                    Value = configuration.Value,
                    Description = configuration.Description,
                    Subject = configuration.Subject,
                    CanBeEdited = configuration.CanBeEdited,
                }).ToList();

                return Ok(configEntity);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while fetching the receivables data.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateConfig(int Id, string value)
        {
            try
            {
                var configData = await _configurationSettings.GetDetailAsync(Id);

                if(configData == null)
                {
                    throw new Exception("Invalid id !!!");
                }

                if (configData.Value == "true")
                {
                    configData.Value = "false";
                }
                else if (configData.Value == "false")
                {
                    configData.Value = "true";
                }
                else
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new Exception("Value cannot be null !!!");
                    }

                    configData.Value = value;
                }

                _configurationSettings.Update(configData);
                await _configurationSettings.SaveAsync();

                return Ok("Data updated !!!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while fetching the receivables data.",
                    Details = ex.Message
                });
            }
        }
    }
}

