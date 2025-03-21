using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMAccReport
    {
        public decimal TotalDebit => AccReportDetail.Select(x => x.Debit).DefaultIfEmpty(0).Sum();

        public decimal TotalCredit => AccReportDetail.Select(x => x.Credit).DefaultIfEmpty(0).Sum();

        public decimal DiffernceAmt => TotalDebit - TotalCredit;

        public IList<VMAccReportDetail> AccReportDetail { get; set; }
    }

    public partial class VMAccReportDetail: VMAccLegder
    {
       
    }

    public class ExpenseSummaryDTO
    {
        public string LedgerName { get; set; }
        public decimal Balance { get; set; }
    }

    public class ExpenseSummaryDetail
    {
        public DateTime? Date { get; set; }
        public string Particulars { get; set; }
        public IList<ExpenseSummaryDTO> Ledgers {  get; set; }
    }
}
