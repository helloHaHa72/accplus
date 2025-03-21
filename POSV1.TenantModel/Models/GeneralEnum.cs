using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreHelpers;

namespace POSV1.TenantModel.Models
{
    public enum EnumVoucherType
    {
        Journal, Income, Expense
    }
    //matching table should not have identity value
    public enum EnumLedgerTypes
    {
        Assets = 1, Liabilities, Income, Expenses
    }

    public enum EnumVoucherStatus
    {
        Approved = 1,
        Pending = 2,
        Rejected = 3,
        //UnApproved = 4,
    }

    public enum EnumERPStatus
    {
        Complete,
        InComplete
    }


    public enum EnumGender
    {
        Male,
        Female,
        Others
    }

    public enum EnumTransactionTypes
    {
        Advance = 1,
        Production = 2,
        ShiftingToDock = 3,
        ShiftingToCounter = 4,
        MonthlyPayroll = 5,
        Settlement = 6
    }

    public enum EnumVoucherTypes
    {
        Journal = 1,
        Income = 2,
        Expense = 3
    }

    public enum EnumFuelType
    {
        Petrol = 1,
        Diesel = 2,
    }

    public enum enumConfigSettingsKeys
    {
        HR_Production_Rate = 1,
        HR_ShiftingToDock_Rate,
        HR_ShiftingToCounter_Rate,
        Accounting_TaxPercent,
        Accounting_Taxable,
        Accounting_VatPercent,

        Accounting_DefaultLedgerForNewProducts,
        Accounting_DefaultLedgerForCustomers,
        Accounting_DefaultLedgerForVendors,
        Accounting_DefaultLedgerForEmployees,
        Accounting_DefaultLedgerForDiscountTaken,
        Accounting_DefaultLedgerForDiscountGiven,
    }

    public static class StringExtensions
    {
        public static string SubstringUpToFirst(this string source, char delimiter)
        {
            int index = source.IndexOf(delimiter);
            return (index < 0) ? source : source.Substring(0, index);
        }
    }

    public static class ConfigSettingsKeyValues
    {
        public static IDictionary<enumConfigSettingsKeys, KeyValuePair<string, string>> _AllKeys = new Dictionary<enumConfigSettingsKeys, KeyValuePair<string, string>>();
        static ConfigSettingsKeyValues()
        {
            foreach (var item in EnumHelper.GetItems<enumConfigSettingsKeys>())
            {
                var key = item.ToString("g").SubstringUpToFirst('_');
                var val = item.ToString("g").Replace(key + "_", "");
                _AllKeys.Add(item, new KeyValuePair<string, string>(key, val));
            }
        }
    }
}
