namespace POSV1.TenantAPI.Models
{
    public partial class VMVendor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Registration_No { get; set; }
        public string Tel_No { get; set; }
        public decimal? Opening_Balance { get; set; }
        public DateTime Registered_Date { get; set; }
        public string Ledger_code { get; set; }
    }

    public partial class CreateVMVendor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Registration_No { get; set; }
        public string Tel_No { get; set; }
        public decimal? Opening_Balance { get; set; }
        public DateTime Registered_Date { get; set; }
        public bool IsVat {  get; set; }
        //public string Number { get; set; }
        //public string Ledger_code { get; set; }
    }

    public partial class VMViewVendorData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Registration_No { get; set; }
        public string Tel_No { get; set; }
        //public string Ledger_code { get; set; }
    }
}
