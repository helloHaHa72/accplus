using Microsoft.EntityFrameworkCore;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.MasterDBModel.Data
{
    public static class AccessListSeeder
    {
        public static async Task SeedAccessListAsync(MainDbContext context)
        {
            var existingTitles = new HashSet<string>(await context.AccessLists.Select(al => al.Title).ToListAsync());

            var newAccessList = new List<AccessList>();

            foreach (EnumSubCategory subCategory in Enum.GetValues(typeof(EnumSubCategory)))
            {
                string subCategoryTitle = subCategory.ToString();

                if (!existingTitles.Contains(subCategoryTitle)) // Only add if it doesn't exist
                {
                    newAccessList.Add(new AccessList
                    {
                        Title = subCategoryTitle,
                        MainHeading = GetMainHeading(subCategory),
                        CreatedBy = "SYSTEM"
                    });
                }
            }

            if (newAccessList.Any())
            {
                await context.AccessLists.AddRangeAsync(newAccessList);
                await context.SaveChangesAsync();
            }
        }

        private static EnumMainHeading GetMainHeading(EnumSubCategory subCategory)
        {
            return subCategory switch
            {
                // Primary SetUp
                EnumSubCategory.USER_ROLES or
                EnumSubCategory.USER_MANAGEMENT or
                EnumSubCategory.MENU_INFO => EnumMainHeading.PRIMARY_SETUP,

                // Charts of Account
                EnumSubCategory.GENERAL_LEDGER or
                EnumSubCategory.LEDGER => EnumMainHeading.CHARTS_OF_ACCOUNT,

                // HR SetUp
                //EnumSubCategory.EMPLOYEE or
                //EnumSubCategory.EMPLOYEES_SALARY => EnumMainHeading.HR_SETUP,

                // Inventory Set Up
                EnumSubCategory.PRODUCT_CATEGORY or
                EnumSubCategory.PRODUCT_UNIT or
                EnumSubCategory.PRODUCTS or
                EnumSubCategory.VENDORS or
                EnumSubCategory.CUSTOMER or
                EnumSubCategory.CUSTOMERS_TYPE or
                EnumSubCategory.VEHICLE_TYPE or
                EnumSubCategory.VEHICLE => EnumMainHeading.INVENTORY_SETUP,

                // HR Transaction
                //EnumSubCategory.ADVANCE_RECORD or
                //EnumSubCategory.PRODUCTION_RECORD or
                //EnumSubCategory.SHIFTING_TO_DOCK or
                //EnumSubCategory.SHIFTING_TO_COUNTER or
                //EnumSubCategory.PAYROLL or
                //EnumSubCategory.EMPLOYEES_SETTLEMENT or
                //EnumSubCategory.FUEL => EnumMainHeading.TRANSACTION,

                // Accounting Transaction
                EnumSubCategory.JOURNAL_VOUCHERS or
                EnumSubCategory.INCOME_VOUCHERS or
                EnumSubCategory.EXPENSE_VOUCHERS => EnumMainHeading.ACCOUNTING_TRANSACTION,

                // Inventory Transaction
                EnumSubCategory.PURCHASE or
                EnumSubCategory.PURCHASE_RETURN or
                //EnumSubCategory.SALES or
                EnumSubCategory.GENERAL_SALES or
                EnumSubCategory.SALES_RETURN or
                EnumSubCategory.PRODUCTION or
                EnumSubCategory.CASH_SETTLEMENT => EnumMainHeading.INVENTORY_TRANSACTION,

                // Accounting Reports
                EnumSubCategory.UNAPPROVED_VOUCHERS or
                EnumSubCategory.APPROVED_VOUCHERS or
                EnumSubCategory.LEDGER_STATEMENT or
                EnumSubCategory.TRIAL_BALANCE or
                EnumSubCategory.BALANCE_SHEET or
                EnumSubCategory.PROFIT_LOSS_REPORT => EnumMainHeading.REPORT,

                // Inventory Reports
                EnumSubCategory.CUSTOMER_SUMMARY or
                EnumSubCategory.VENDOR_SUMMARY or
                EnumSubCategory.ITEMWISE_TRANSACTION or
                EnumSubCategory.STOCK_SUMMARY_REPORT or
                EnumSubCategory.STOCK_STATEMENT_REPORT or
                EnumSubCategory.SALES_REPORT or
                EnumSubCategory.PURCHASE_REPORT or
                EnumSubCategory.VAT_REPORT or
                //EnumSubCategory.VEHICLE_LOG_BOOK or
                EnumSubCategory.INVENTORY_LOGBOOK => EnumMainHeading.REPORT,

                // HR Reports
                //EnumSubCategory.FUEL_BOOK_REPORT => EnumMainHeading.REPORT,

                // Settings
                EnumSubCategory.TAX or
                //EnumSubCategory.WAGES or
                EnumSubCategory.GENERAL_SETTINGS => EnumMainHeading.SETTINGS,

                _ => throw new ArgumentOutOfRangeException(nameof(subCategory), $"No mapping defined for {subCategory}")
            };
        }
    }

}
