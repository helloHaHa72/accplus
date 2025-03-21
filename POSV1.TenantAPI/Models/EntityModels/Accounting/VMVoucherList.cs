namespace POSV1.TenantAPI.Models
{
    public partial class VMVoucherList
    {
        public  string VoucherNo { get; set; }

        public  string VoucherType { get; set; }

        public  DateTime ValueDate { get; set; }

        public  string ManualVno { get; set; }

        public  string Remarks { get; set; }

        public  decimal TotalCredit { get; set; }

        public  decimal TotalDebit { get; set; }

        public  string  Status { get; set; }
        public string CreatedName { get; set; }
        public  string UpdatedName { get; set; }
    }

    public enum Status
    {
        Approved = 1 ,
        Pending = 2 ,
        Rejected =3 ,
        UnApproved =4,
    }
}


