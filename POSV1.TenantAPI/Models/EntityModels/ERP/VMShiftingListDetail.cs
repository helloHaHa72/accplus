namespace POSV1.TenantAPI.Models
{
    public partial class VMShiftingListDetail
    {
        public virtual int ID { get; set; }

        public virtual DateTime Transaction_Date { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string Verified_By { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual string WageType { get; set; }

        public virtual int WageTypeId { get; set; }

        public virtual int? HeadId { get; set; }

        public virtual decimal Total_Amt { get; set; }

        public virtual IList<VMShiftingDetail> VMShiftingDetail { get; set; }
    }

    public partial class VMShiftingDetail
    {
        public virtual int ID { get; set; }

        public virtual int EmployeeID { get; set; }

        public virtual string EmployeeName { get; set; }

        public virtual string? Ref_No { get; set; }
        public virtual decimal Rate { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual string Remarks { get; set; }

        public virtual bool Status { get; set; }
    }
}
