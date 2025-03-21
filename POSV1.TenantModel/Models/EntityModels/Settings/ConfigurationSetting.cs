using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace POSV1.TenantModel.Models.EntityModels.Settings
{
    public class ConfigurationSetting : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [Column(TypeName = "nvarchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Value { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Subject { get; set; }
        public bool? CanBeEdited { get; set; }

        public static void SeedDefault(MainDbContext dbContext)
        {
            List<ConfigurationSetting> configs = new List<ConfigurationSetting>()
            {
                new ConfigurationSetting()
                {
                    Name=EnumConfigSettings.ApplySalesVat.ToString(),
                    Description="Settings to Apply/Disable VAT for Sales",
                    Value="false",
                    CreatedName="system",
                    CanBeEdited=true
                },
                new ConfigurationSetting()
                {
                    Name=EnumConfigSettings.AutoApproveVoucher.ToString(),
                    Description="Settings to Auto Approve Voucher",
                    Value="false",
                    CreatedName="system",
                    CanBeEdited=true
                },
            };

            foreach (var configSetting in configs)
            {
                try
                {
                    if (!dbContext.ConfigurationSettings.Any(x => x.Name == configSetting.Name))
                    {
                        dbContext.Add(configSetting);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
