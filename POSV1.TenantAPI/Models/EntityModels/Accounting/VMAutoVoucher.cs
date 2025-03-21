namespace POSV1.TenantAPI.Models
{

    public class VMAutoVoucher
    {
        public virtual string VoucherNo { get; set; }

        public virtual DateTime ValueDate { get; set; }
        public virtual string ManualVno { get; set; }
        public virtual decimal TotalCredit { get; set; }
        public virtual decimal TotalDebit { get; set; }
        public virtual string UpdatedName { get; set; }
        public virtual IList<VMAutoVoucherDetail> VMAutoVoucherDetail { get; set; }
    }

    public partial class VMAutoVoucherDetail
    {
        public string Led_Code { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }

    }
}
