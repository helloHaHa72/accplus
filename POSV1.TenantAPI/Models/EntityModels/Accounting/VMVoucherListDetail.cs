namespace POSV1.TenantAPI.Models
{
    public partial class VMVoucherListDetail
    {
        public string ID { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherType { get; set; }
        public DateTime ValueDate { get; set; }
        public string ManualVno { get; set; }
        public int? ContraLedgerId { get; set; }
        public string? ContraLedgerName { get; set; }
        public string Remarks { get; set; }
        public decimal TotalCredit => VMDetails.Select(x => x.Credit).DefaultIfEmpty(0).Sum();
        public decimal TotalDebit => VMDetails.Select(x => x.Debit).DefaultIfEmpty(0).Sum();
        public string ChqNo { get; set; }
        public string Status { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedName { get; set; }
        public IList<VMDetails> VMDetails { get; set; }
    }

    public partial class VMDetails
    {
        public int ID { get; set; }
        public long LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string LedgerNameCode { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Description { get; set; }
        public string ChqNo { get; set; }
        public decimal Balance { get; set; }
    }
}
