using System;
using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public static class ConfigSettingsKeyValues
    {
        public static IDictionary<enumConfigSettingsKeys, KeyValuePair<string, string>> _AllKeys = new Dictionary<enumConfigSettingsKeys, KeyValuePair<string, string>>();
        static ConfigSettingsKeyValues()
        {
            _AllKeys.Add(enumConfigSettingsKeys.HR_Production_Rate, new KeyValuePair<string, string>("HR", "Production_Rate"));
            _AllKeys.Add(enumConfigSettingsKeys.HR_ShiftingToDock_Rate, new KeyValuePair<string, string>("HR", "ShiftingToDock_Rate"));
            _AllKeys.Add(enumConfigSettingsKeys.HR_ShiftingToCounter_Rate, new KeyValuePair<string, string>("HR", "ShiftingToCounter_Rate"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_TaxPercent, new KeyValuePair<string, string>("Accounting", "TaxPercent"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_Taxable, new KeyValuePair<string, string>("Accounting", "Taxable"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_VatPercent, new KeyValuePair<string, string>("Accounting", "VatPercent"));
      
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_DefaultLedgerForNewProducts, new KeyValuePair<string, string>("Accounting", "DefaultLedgerForNewProducts"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_DefaultLedgerForCustomers, new KeyValuePair<string, string>("Accounting", "DefaultLedgerForCustomers"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_DefaultLedgerForVendors, new KeyValuePair<string, string>("Accounting", "DefaultLedgerForVendors"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_DefaultLedgerForEmployees, new KeyValuePair<string, string>("Accounting", "DefaultLedgerForEmployees"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_DefaultLedgerForDiscountTaken, new KeyValuePair<string, string>("Accounting", "DefaultLedgerForDiscountTaken"));
            _AllKeys.Add(enumConfigSettingsKeys.Accounting_DefaultLedgerForDiscountGiven, new KeyValuePair<string, string>("Accounting", "DefaultLedgerForDiscountGiven"));
     
        }

    }


}
