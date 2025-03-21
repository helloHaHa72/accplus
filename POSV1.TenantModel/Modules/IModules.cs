using BaseAppSettings;
using Microsoft.AspNetCore.Identity;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using System;

namespace POSV1.TenantModel.Modules
{
    public class AccountingModule : IModules<MainDbContext>
    {
        public ModuleInfo GetInfo()
        {
            return new ModuleInfo()
            {
                ID = EnumModules.Accounting,
                ModuleName = "General Accouting",
                IsActive = true,
            };
        }

        public void SeedData(MainDbContext dbContext)
        {
            //lets seed data for voucher typoes, ledger types

            foreach (EnumVoucherTypes item in Enum.GetValues<EnumVoucherTypes>())
            {
                var dbRec = dbContext.vou01voucher_types.Find((int)item);
                if (dbRec != null) { continue; }

                //lets create
                dbContext.vou01voucher_types.Add(new vou01voucher_types()
                {
                    vou01uin = (int)item,
                    vou01title = item.ToString("g"),
                    vou01last_no = 1,
                    vou01prefix = item.ToString("g").Substring(0, 1) + "V",
                });
            }
            foreach (EnumLedgerTypes item in Enum.GetValues<EnumLedgerTypes>())
            {
                var dbRec = dbContext.led05ledger_types.Find((int)item);
                if (dbRec != null) { continue; }

                //lets craete
                dbContext.led05ledger_types.Add(new led05ledger_types()
                {
                    led05uin = (int)item,
                    led05title = item.ToString("g"),
                    led05add_dr = (item == EnumLedgerTypes.Assets || item == EnumLedgerTypes.Expenses),
                    //vou01prefix = item.ToString("g").Substring(0, 1) + "V",
                    led05title_nep = item.ToString("g")

                });
            }
            dbContext.SaveChanges();

        }
    }
    public class InventoryModule : IModules<MainDbContext>
    {
        public ModuleInfo GetInfo()
        {
            return new ModuleInfo()
            {
                ID = EnumModules.Inventory,
                ModuleName = "Inventory Module",
                IsActive = true,
            };
        }

        public void SeedData(MainDbContext dbContext)
        {
            //lets seed data for basic Units, basic product type
        }
    }
}
