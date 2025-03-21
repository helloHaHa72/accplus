using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantAPI.Models
{
    public partial class VMFuelLogBook
    {
        public int VehicleId { get; set; }
        public string Vehicle_No { get; set; }
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
}
