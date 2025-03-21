namespace POSV1.TenantAPI.Models
{
    public partial class VMShiftingList
    {
        public int ID { get; set; }

        public DateTime Transaction_Date { get; set; }

        public string Remarks { get; set; }

        public string Verified_By { get; set; }

        public decimal Total_Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal Total_Amt { get; set; }
    }
}
