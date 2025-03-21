using POSV1.TenantModel.Models;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantAPI.Models
{
    public partial class VMEmployeeSalary
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public decimal Basic { get; set; }
        public decimal Grade { get; set; }
        public virtual decimal Sub_Total => Basic + Grade;
        public decimal Allowance { get; set; }
        public decimal Deductions { get; set; }
        public virtual decimal Taxable_Total => Sub_Total + Allowance - Deductions;
        public decimal Tax_Percent => AppSettingsWrapper.AppSettings.TaxPercent;
        public bool Is_Taxable => AppSettingsWrapper.AppSettings.Taxable;
        public virtual decimal Tax_Amt => Is_Taxable ? (Taxable_Total * Tax_Percent) / 100 : 0;
        public virtual decimal Net_Payable_Amt => Taxable_Total - Tax_Amt;
    }

    public partial class VMEmployeeSalaryList
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public ViewEmployeeDetail EmployeeDetail {  get; set; }
        public decimal Basic { get; set; }
        public decimal Grade { get; set; }
        public virtual decimal Sub_Total { get; set; }
        public decimal Allowance { get; set; }
        public decimal Deductions { get; set; }
        public virtual decimal Taxable_Total { get; set; }
        public decimal Tax_Percent { get; set; }
        public bool Is_Taxable { get; set; }
        public virtual decimal Tax_Amt { get; set; }
        public virtual decimal Net_Payable_Amt { get; set; }
    }

    public class ViewEmployeeDetail
    {
        public string Email { get; set; }
        public string PhoneNumber {  get; set; }
        public string Address {  get; set; }
    }
}
