namespace POSV1.TenantAPI.Models
{
    public partial class VMCustomerTypeto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double? DiscountPercentage { get; set; }
    }

    public partial class VMViewCustomer
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TelePhone_No { get; set; }
        public decimal OpeningBalance { get; set; }
        public string Registration_No { get; set; }
        public VMCustomerTypeto? CustomerType { get; set; }
        public DateTime Registered_Date { get; set; }
        public string Ledger_code { get; set; }
        public bool Status { get; set; }

    }

    public partial class VMCustomer
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TelePhone_No { get; set; }
        public decimal OpeningBalance { get; set; }
        public string Registration_No { get; set; }
        public int? CustomerTypeId { get; set; }
        public DateTime Registered_Date { get; set; }
        //public string Ledger_code { get; set; }
        public bool Status { get; set; }
        public bool IsVat { get; set; }
        //public string Number { get; set; }

    }

    public partial class VMViewCustomerData
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TelePhone_No { get; set; }
        public string Registration_No { get; set; }
        public int? CustomerTypeId { get; set; }
        public DateTime Registered_Date { get; set; }
    }
}
