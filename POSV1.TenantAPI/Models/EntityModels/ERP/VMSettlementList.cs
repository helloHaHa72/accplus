namespace POSV1.TenantAPI.Models
{
    public partial class VMSettlementList
    { 
        public  int ID { get; set; }
        public  DateTime _Date { get; set; }
        public  string Settlement_Date => _Date.ToString("yyyy/MM/dd");
        public  string Remarks { get; set; }
        public  string Verified_By { get; set; }
    }
}
