namespace POSV1.TenantAPI.Models
{
    public partial class VMSupplier
    {
        public int? ID { get; set; }
        public string Address { get; set; }
        public string TelPhone_No { get; set; }
        public DateTime? RegDate { get; set; }
        public string RegNo { get; set; }
        public decimal Balance { get; set; }
    }
}
