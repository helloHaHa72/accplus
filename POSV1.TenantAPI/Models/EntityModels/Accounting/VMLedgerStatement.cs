using POSV1.TenantAPI.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class ReqLedgerStatement
    {
        public virtual int LedgerID { get; set; }

        public virtual DateTime? FromDate { get; set; }

        public virtual DateTime? ToDate { get; set; }
    }
    public partial class VMLedgerStatement
    {
        public int LedgerID { get; set; }
        public string LedgerName { get; set; }
        public int? ParentGLID { get; set; }
        public string ParentGLName { get; set; }
        public string LedgerCode { get; set; }
        public string ParentGlCode { get; set; }
        public decimal TotalDebit => VMLedgerStmtDetail.Select(x => x.Debit).DefaultIfEmpty(0).Sum();
        public decimal TotalCredit => VMLedgerStmtDetail.Select(x => x.Credit).DefaultIfEmpty(0).Sum();
        public decimal OpeningBalance => VMLedgerStmtDetail.FirstOrDefault() != null ? VMLedgerStmtDetail.First().Balance : 0;
        public decimal ClosingBalance => VMLedgerStmtDetail.LastOrDefault() != null ? VMLedgerStmtDetail.Last().Balance : 0;
        public decimal TotalDifference => TotalDebit - TotalCredit;
        public IList<VMLedgerStmtDetail> VMLedgerStmtDetail { get; set; }
    }
    public partial class VMLedgerStmtDetail
    {
        public virtual int ID { get; set; }

        public virtual string VoucherNo { get; set; }

        public virtual DateTimeOffset ValueDate { get; set; }

        public virtual string Narration { get; set; }

        public virtual decimal Debit { get; set; }

        public virtual decimal Credit { get; set; }

        public virtual decimal Balance { get; set; }

        public virtual string UpdatedName { get; set; }

    }
}




