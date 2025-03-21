using System.ComponentModel.DataAnnotations.Schema;
using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMVehicleWithTxnDetails
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleType { get; set; }
        public EnumFuelType FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string Vehicle_No { get; set; }
        public string Vehicle_Model { get; set; }
        public IList<VMVecFuelLogBook> FuelLogBook { get; set; }
        public IList<VMVecVehicleLogBook> VehicleLogBook { get; set; }
    }

    public partial class VMVecFuelLogBook
    {
        [NotMapped]
        public DateTime _date { get; set; }
        public string Date => _date.ToString("yyyy/MM/dd");
        public string Time => _date.ToString("hh/mm tt");
        public int PumpId { get; set; }
        public string PumpName { get; set; }
        public decimal Quantity { get; set; }
        public string Token_No { get; set; }
        public string Remarks { get; set; }

    }

    public partial class VMVecVehicleLogBook
    {
        public string Destination { get; set; }
        public DateTime _TxnDate { get; set; }
        public string TxnDate => _TxnDate.ToString("yyyy/MM/dd hh/mm tt");
        public double Txn_Qty { get; set; }
        public string Remarks { get; set; }
    }

}
