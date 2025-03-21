using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMVehicle
    {
        public virtual int VehicleTypeId { get; set; }
        public virtual EnumFuelType FuelType { get; set; }
        public virtual string Vehicle_No { get; set; }
        public virtual string Vehicle_Model { get; set; }
    }

    public partial class VMVehicleList
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleType { get; set;}
        public EnumFuelType FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string Vehicle_No { get; set; }
        public  string Vehicle_Model { get; set; }
    }

}
