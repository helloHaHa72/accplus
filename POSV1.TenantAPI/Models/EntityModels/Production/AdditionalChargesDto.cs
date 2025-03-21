namespace POSV1.TenantAPI.Models.EntityModels.Production
{
    public class AdditionalChargesDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; }
    }

    public class AdditionalChargesCreateDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; }
    }
}
