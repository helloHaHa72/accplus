using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSV1.TenantAPI.Models
{
    public class VMEditSettings
    {
        public decimal Production_Rate { get; set; }
        public decimal ShiftingToDock_Rate { get; set; }
        public decimal ShiftingToCounter_Rate { get; set; }
        public decimal TaxPercent { get; set; }
        public bool Taxable { get; set; }
        public decimal VatPercent { get; set; }
        public string DefaultLedgerForNewProducts { get; set; }
        public string DefaultLedgerForCustomers { get; set; }
        public string DefaultLedgerForVendors { get; set; }
        public string DefaultLedgerForEmployees { get; set; }
        public string DefaultLedgerForDiscountTaken { get; set; }
        public string DefaultLedgerForDiscountGiven { get; set; }
       
    }
}