namespace POSV1.TenantAPI.Models
{
    public partial class VMVehicleLogBook
    {
        public int? VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Destination { get; set; }
        public DateTime _TxnDate { get; set; }
        public string TxnDate => _TxnDate.ToString("yyyy/MM/dd hh/mm tt");
        public double Txn_Qty { get; set; }
        public string Remarks { get; set; }
    }
}
