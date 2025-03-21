using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMFuelToken
    {
        public string Ref_No { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public EnumFuelType FuelType { get; set; }
        public int PumpId { get; set; }
        public DateTime Issued_Date { get; set; }
        public decimal Quantity { get; set; }
        public string Remarks { get; set; }
    }

    public partial class VMFuelTokenList
    {
        public int Id { get; set; }
        public string Ref_No { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public EnumFuelType FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public int PumpID { get; set; }
        public string PumpName { get; set; }
        public string Issued_By { get; set; }
        public DateTime _Issued_Date { get; set; }
        public string Issued_Date => _Issued_Date.ToString("yyyy/MM/dd");
        public string Issued_Time => _Issued_Date.ToString("hh:mm tt");
        public decimal Quantity { get; set; }
        public string Remarks { get; set; }
    }
}
