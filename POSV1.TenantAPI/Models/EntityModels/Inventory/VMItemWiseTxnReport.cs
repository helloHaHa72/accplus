using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantAPI.Models
{
    public partial class VMItemWiseTxnReport
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public DateTime _Txn_Date { get; set; }
        public string TxnDate => _Txn_Date.ToString("yyyy/MM/dd");
        public string Ref_No { get; set; }
        public decimal Rate { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Net_Amt { get; set; }
    }
}
