namespace POSV1.TenantAPI.Models
{
    public partial class VMTransactionList
    {
        public  int ID { get; set; }
        public  string Transaction_Date { get; set; }
        public  string Remarks { get; set; }

        public  string Verified_By { get; set; }

        public  decimal Total_Amt { get; set; }

    }
}
