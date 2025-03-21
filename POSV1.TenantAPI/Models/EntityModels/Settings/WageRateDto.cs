using POSV1.TenantModel;

namespace POSV1.TenantAPI.Models.EntityModels.Settings
{
    public class WageRateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IdentificationCode { get; set; }
        public double Value { get; set; }
        public string WageType { get; set; }
        public bool CanBeDeleted { get; set; }
    }

    public class CreateWageRateDto
    {
        public string Title { get; set; }
        //public string IdentificationCode { get; set; }
        public double Value { get; set; }
        public EnumWageType WageType { get; set; }
    }
}
