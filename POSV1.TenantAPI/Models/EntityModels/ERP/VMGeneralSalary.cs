using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantAPI.Models
{
    public class VMGeneralSalary
    {
        public decimal Basic { get; set; }
        public decimal Grade { get; set; }
        public decimal Allowance { get; set; }
        public decimal Deductions { get; set; }
        public virtual decimal SubTotal => Basic + Grade;
        public virtual decimal TaxableTotal => Basic + Allowance - Deductions;
        public decimal TaxPercent => AppSettingsWrapper.AppSettings.TaxPercent;  //= 1; settings read and set
        public bool Taxable => AppSettingsWrapper.AppSettings.Taxable; //= true; settings read and set
        public virtual decimal TaxAmt => Taxable ? (TaxableTotal * TaxPercent) / 100 : 0;
        public virtual decimal NetPayableAmount => TaxableTotal - TaxAmt;
    }

    public class DesignationSalarySetup : VMGeneralSalary //should be db entity
    {
        [Key]
        public int Id { get; set; }
        public int DesignationId { get; set; }
    }

    public class EmployeeSalarySetup : VMGeneralSalary //should be db entity
    {
        public int EmployeeId { get; set; }
        public VMGeneralSalary AdditionalSalary { get; set; }
        public override decimal SubTotal => base.SubTotal + AdditionalSalary.SubTotal;
        public override decimal TaxableTotal => base.TaxableTotal + AdditionalSalary.TaxableTotal;
        public override decimal TaxAmt => base.TaxAmt + AdditionalSalary.TaxAmt;
        public override decimal NetPayableAmount => base.NetPayableAmount + AdditionalSalary.NetPayableAmount;

    }

    public class VMGeneralSalaryList
    {
        public int ID { get; set; }
        public decimal Basic { get; set; }
        public decimal Grade { get; set; }
        public decimal Allowance { get; set; }
        public decimal Deductions { get; set; }
        public virtual decimal SubTotal {get; set; }
        public virtual decimal TaxableTotal {get; set; }
        public decimal TaxPercent { get; set; }
        public bool Taxable { get; set; } 
        public virtual decimal TaxAmt { get; set; }
        public virtual decimal NetPayableAmount {get; set; }
    }
}
