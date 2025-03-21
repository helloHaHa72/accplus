using System.Text.Json.Serialization;

namespace POSV1.TenantAPI.Models
{

    public partial class VMTransactionListDetail
    {
        public virtual int ID { get; set; }

        public virtual DateTime Transaction_Date { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string Verified_By { get; set; }

        public virtual decimal Total_Amt {  get; set; }
        //[Json]
        public virtual IList<VMDetail> VMDetail { get; set; }  // VMTransactionDetails
    }

    public partial class VMDetail
    {
        public virtual int ID { get; set; }

        public virtual int EmployeeID { get; set; }

        public virtual string EmployeeName { get; set; }

        public virtual string? Ref_No { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string LedgerRefrenceNumber { get; set; }

        public virtual bool Status { get; set; }
    }
}
