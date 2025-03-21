namespace POSV1.TenantAPI.Models
{
    public class VMProduction
    {
        public  DateTime Transaction_Date { get; set; }

        public  string Remarks { get; set; }

        public int? HeadId { get; set; }

        //public virtual decimal Total_Amt => VMProductionDetails.Select(x => x.Amount).Sum();

        public virtual IList<VMProductionDetails> VMProductionDetails { get; set; }
    }

    public partial class VMProductionDetails
    {
        public  int ID { get; set; }

        public  int EmployeeID { get; set; }

        public  string? Ref_No { get; set; }

        public  decimal Quantity { get; set; }

        //public decimal Rate { get; set; }

        //public decimal Amount => Quantity * Rate;

        public  string Remarks { get; set; }

        public virtual bool Status { get; set; }
    }

}
