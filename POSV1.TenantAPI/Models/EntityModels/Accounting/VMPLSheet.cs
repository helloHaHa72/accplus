namespace POSV1.TenantAPI.Models
{
    public partial class VMPLSheet
    {
        public virtual decimal TotalDebit => VMPLSheetDetail.Select(x => x.Debit).DefaultIfEmpty(0).Sum();

        public virtual decimal TotalCredit => VMPLSheetDetail.Select(x => x.Credit).DefaultIfEmpty(0).Sum();

        public virtual decimal DiffernceAmt => TotalDebit - TotalCredit;

        public IList<VMPLSheetDetail> VMPLSheetDetail { get; set; }
    }

    public partial class VMPLSheetDetail
    {
        public virtual int ID { get; set; }

        public virtual int LedgerId { get; set; }

        public virtual string LedgerName { get; set; }

        public virtual decimal Balance { get; set; }

        public virtual decimal Debit => Balance < 0 ? Balance* -1 : 0;

        public virtual decimal Credit => Balance > 0 ? Balance : 0;
    }
}
