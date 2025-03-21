namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class VMCustomerType
    {
        public string Title { get; set; } = null!;
        public double? DiscountPercentage { get; set; }
    }

    public class ViewCustomerType : VMCustomerType
    {
        public int? Id { get; set; }
    }
}
