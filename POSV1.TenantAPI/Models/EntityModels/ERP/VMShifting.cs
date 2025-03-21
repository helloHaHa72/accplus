using POSV1.TenantModel;

namespace POSV1.TenantAPI.Models
{
    public partial  class VMShifting
    {
        public DateTime Transaction_Date { get; set; }

        public string Remarks { get; set; }

        public decimal Rate { get; set; }

        public EnumWageType WageType { get; set; }

        public int? HeadId { get; set; }

        public virtual IList<VMShiftingDetails> VMShiftingDetails { get; set; }
    }

    public partial class VMShiftingDetails
    {
        public int ID { get; set; }

        public int EmployeeID { get; set; }

        public string? Ref_No { get; set; }

        public decimal Quantity { get; set; }

        public string Remarks { get; set; }

        public virtual bool Status { get; set; }
    }
}
