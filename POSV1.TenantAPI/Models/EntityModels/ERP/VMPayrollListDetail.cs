﻿namespace POSV1.TenantAPI.Models
{
    public partial class VMPayrollListDetail
    {
        public virtual int ID { get; set; }
        public virtual DateTime Transaction_Date { get; set; }
        public DateTime Month { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string Verified_By { get; set; }
        public virtual decimal Total_Amt { get; set; }
        public virtual IList<VMPayrollDetail> VMPayrollDetail { get; set; }
    }

    public partial class VMPayrollDetail
    {
        public virtual int ID { get; set; }
        public virtual int EmployeeID { get; set; }
        public virtual string EmployeeName { get; set; }
        public virtual string? Ref_No { get; set; }
        public decimal Salary_Amt { get; set; }
        public decimal Payable_Amt { get; set; }
        public decimal Paid_Amt { get; set; }
        public virtual string Remarks { get; set; }
        public virtual bool Status { get; set; }
    }

}
