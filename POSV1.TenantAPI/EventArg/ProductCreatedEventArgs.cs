using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantAPI.EventArg
{
    public class ProductCreatedEventArgs : EventArgs
    {
        public pro02products CreatedProduct { get; set; }
    }
    public static class EventManager
    {
        public static event EventHandler<ProductCreatedEventArgs> ProductCreated;

        public static void OnProductCreated(pro02products createdProduct)
        {
            ProductCreated?.Invoke(null, new ProductCreatedEventArgs { CreatedProduct = createdProduct });
        }
    }
}
