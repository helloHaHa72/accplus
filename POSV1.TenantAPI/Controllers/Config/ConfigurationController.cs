using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static POSV1.TenantAPI.Models.VMEditConfig;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigValuesByEnumRepo _configRepo;

        public ConfigurationController(IConfigValuesByEnumRepo configRepo)
        {
            _configRepo = configRepo;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetConfigurations()
        {
            try
            {
                // Retrieve all configuration values from the database
                var allConfigurations = _configRepo.GetList();

                var configEntity = allConfigurations.Select(configuration => new VMConfigurationList
                {
                    Key = configuration.cfg01key,
                    Value = configuration.cfg01value
                }).ToList();

                return Ok(configEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateConfiguration(VMConfiguration newConfig)
        {
            try
            {
                var newConfiguration = new cfg01configurations
                {
                    cfg01module = newConfig.module,
                    cfg01key = newConfig.Key.ToString(),
                    cfg01value = newConfig.Value,

                    cfg01created_name = "Admin",
                    cfg01created_date = DateTime.Now,
                    cfg01updated_date = DateTime.Now,
                    cfg01updated_name = "Admin"
                };

                _configRepo.Insert(newConfiguration);
                await _configRepo.SaveAsync();

                return Ok("Configuration created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetEditSettings()
        {
            try
            {
                var data = new VMEditSettings
                {
                    Production_Rate = _configRepo.GetDecimal(enumConfigSettingsKeys.HR_Production_Rate),
                    ShiftingToDock_Rate = _configRepo.GetDecimal(enumConfigSettingsKeys.HR_ShiftingToDock_Rate),
                    ShiftingToCounter_Rate = _configRepo.GetDecimal(enumConfigSettingsKeys.HR_ShiftingToCounter_Rate),
                    TaxPercent = _configRepo.GetDecimal(enumConfigSettingsKeys.Accounting_TaxPercent),
                    Taxable = _configRepo.GetBool(enumConfigSettingsKeys.Accounting_Taxable),
                    VatPercent = _configRepo.GetDecimal(enumConfigSettingsKeys.Accounting_VatPercent),
                    DefaultLedgerForNewProducts = _configRepo.GetString(enumConfigSettingsKeys.Accounting_DefaultLedgerForNewProducts),
                    DefaultLedgerForCustomers = _configRepo.GetString(enumConfigSettingsKeys.Accounting_DefaultLedgerForCustomers),
                    DefaultLedgerForVendors = _configRepo.GetString(enumConfigSettingsKeys.Accounting_DefaultLedgerForVendors),
                    DefaultLedgerForEmployees = _configRepo.GetString(enumConfigSettingsKeys.Accounting_DefaultLedgerForEmployees),
                    DefaultLedgerForDiscountTaken = _configRepo.GetString(enumConfigSettingsKeys.Accounting_DefaultLedgerForDiscountTaken),
                    DefaultLedgerForDiscountGiven = _configRepo.GetString(enumConfigSettingsKeys.Accounting_DefaultLedgerForDiscountGiven),
                };
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditConfiguration([FromBody] VMEditSettings data)
        {
            try
            {
                SetDbValues(data);

                return Ok(new { Message = "Configuration Updated Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }
        }

        private void SetDbValues(VMEditSettings data)
        {
            _configRepo.Update(enumConfigSettingsKeys.HR_Production_Rate, data.Production_Rate);
            _configRepo.Update(enumConfigSettingsKeys.HR_ShiftingToDock_Rate, data.ShiftingToDock_Rate);
            _configRepo.Update(enumConfigSettingsKeys.HR_ShiftingToCounter_Rate, data.ShiftingToCounter_Rate);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_TaxPercent, data.TaxPercent);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_Taxable, data.Taxable);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_VatPercent, data.VatPercent);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_DefaultLedgerForNewProducts, data.DefaultLedgerForNewProducts);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_DefaultLedgerForCustomers, data.DefaultLedgerForCustomers);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_DefaultLedgerForVendors, data.DefaultLedgerForVendors);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_DefaultLedgerForEmployees, data.DefaultLedgerForEmployees);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_DefaultLedgerForDiscountTaken, data.DefaultLedgerForDiscountTaken);
            _configRepo.Update(enumConfigSettingsKeys.Accounting_DefaultLedgerForDiscountGiven, data.DefaultLedgerForDiscountGiven);
        }

    }
}
