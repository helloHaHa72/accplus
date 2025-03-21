using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMBalanceSheet
    {
        public decimal TotalDebit => VMBalanceSheetDetail.Select(x => x.Debit).DefaultIfEmpty(0).Sum();

        public decimal TotalCredit => VMBalanceSheetDetail.Select(x => x.Credit).DefaultIfEmpty(0).Sum();

        public decimal DiffernceAmt => TotalDebit - TotalCredit;

        public IList<VMBalanceSheetDetail> VMBalanceSheetDetail { get; set; }
    }

    public partial class VMAccLegder
    {
        public EnumLedgerTypes TypeId { get; set; }
        public string LedgerType { get; set; }
        public int LedgerId { get; set; }
        public string LedgerCode { get; set; }
        public string LedgerName { get; set; }  
        public string DisplayField => $"{LedgerName} [{LedgerCode}]";
        public decimal Balance { get; set; }
        public bool AddDr => (TypeId == EnumLedgerTypes.Assets || TypeId == EnumLedgerTypes.Expenses);
        public decimal Debit => AddDr ? (Balance > 0 ? Balance : 0) : (Balance < 0 ? Balance * -1 : 0);
        public decimal Credit => !AddDr ? (Balance > 0 ? Balance : 0) : (Balance < 0 ? Balance * -1 : 0);
    }

    public partial class VMBalanceSheetDetail : VMAccLegder
    {

    }
}
