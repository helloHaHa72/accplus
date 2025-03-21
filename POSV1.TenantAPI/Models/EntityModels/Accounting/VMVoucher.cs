namespace POSV1.TenantAPI.Models
{
    public partial class VMVoucher
    {
        public virtual decimal Amount { get; set; }
        public virtual DateTime ValueDate { get; set; }
        public virtual string Remarks { get; set; }
        public virtual Status Status { get; set; }
        public virtual string ManualVno { get; set; }
        public virtual string ChqNo { get; set; }
        public int? ContraLedgerId { get; set; }
        public virtual decimal TotalCredit => VMVoucherDetailCreate.Select(x => x.Credit).DefaultIfEmpty(0).Sum();
        public virtual decimal TotalDebit => VMVoucherDetailCreate.Select(x => x.Debit).DefaultIfEmpty(0).Sum();
        public virtual IList<VMVoucherDetail> VMVoucherDetailCreate { get; set; }
    }

    public partial class VMVoucherDetail
    {
        public int ID { get; set; }
        public virtual int LedgerId { get; set; }
        public virtual decimal Debit { get; set; }
        public virtual decimal Credit { get; set; }
        public virtual string Description { get; set; }
        public virtual string ChqNo { get; set; }
        public virtual decimal Balance => Credit - Debit;
    }
}
