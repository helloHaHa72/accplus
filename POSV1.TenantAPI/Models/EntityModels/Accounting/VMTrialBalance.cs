using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMTrialBalance
    {
        public virtual decimal TotalDebit => VMTrialBalanceDetail.Select(x => x.Debit).DefaultIfEmpty(0).Sum();

        public virtual decimal TotalCredit => VMTrialBalanceDetail.Select(x => x.Credit).DefaultIfEmpty(0).Sum();

        public virtual decimal DiffernceAmt => TotalDebit - TotalCredit;

        public IList<VMTrialBalanceDetail> VMTrialBalanceDetail { get; set; }
    }

    public partial class VMTrialBalanceDetail
    {
        public virtual int ID { get; set; }

        public virtual int LedgerId { get; set; }

        public virtual string LedgerName { get; set; }

        public virtual decimal Balance { get; set; }

        public virtual decimal Debit { get; set; }

        public virtual decimal Credit { get; set; } 
    }
}
