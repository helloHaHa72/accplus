using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantAPI.Models
{
    public static class AppSettingsWrapper
    {
        public static AppSettings AppSettings { get; set; }
    }
    public class AppSettings
    {
        public decimal Production_Rate { get; set; }
        public decimal ShiftingToDock_Rate { get; set; }
        public decimal ShiftingToCounter_Rate { get; set; }
        public decimal TaxPercent { get; set; }
        public bool Taxable { get; set; }
        public double VatPercent => 0;
    }

    //public enum EnumLedgerTypes
    //{
    //    Liabilities = 1, Assets, Income, Expenses
    //}

    //public class VMGeneralLedger
    //{
    //    [Required]
    //    public string Code { get; set; }
    //    [Required]
    //    public string GLName { get; set; }
    //    [Required]
    //    public EnumLedgerTypes LedgerType { get; set; }
    //    //[Required]
    //    public int? ParentGlHead { get; set; }
    //}
}
