using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public class ta01taxsettlement : Auditable
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal taxPercentage {  get; set; }
        public bool IsForTDs { get; set; } = true;
        public bool CanBeDeleted { get; set; } = true;

        public static void SeedTaxDefault(MainDbContext dbContext)
        {
            List<ta01taxsettlement> configs = new List<ta01taxsettlement>()
            {
                new ta01taxsettlement()
                {
                    Title = "VAT",
                    taxPercentage = 13,
                    CanBeDeleted = false,
                    IsForTDs = false,
                    CreatedName = "system",
                    DateCreated = DateTime.UtcNow
                },
                new ta01taxsettlement()
                {
                    Title = "TDS",
                    taxPercentage = 1,
                    CanBeDeleted = false,
                    IsForTDs = true,
                    CreatedName = "system",
                    DateCreated = DateTime.UtcNow
                },
            };

            foreach (var configSetting in configs)
            {
                try
                {
                    var existingRecords = dbContext.ta01taxsettlement
                        .Where(x => x.Title == configSetting.Title)
                        .ToList();

                    if (existingRecords.Any())
                    {
                        dbContext.ta01taxsettlement.RemoveRange(existingRecords);
                        dbContext.SaveChanges();
                    }

                    dbContext.Add(configSetting);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }
}
