namespace POSV1.TenantAPI.Models.EntityModels.Production
{
    public class PurchaseAdditionalChargesDto
    {
        public int[] PurchaseIds { get; set; } = Array.Empty<int>();
        public IList<ChargeData> ChargeData { get; set; }
    }

    public class ChargeData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }

    public class ViewChargeData : ChargeData
    {
        public int Id { get; set; }
    }

    //public class PurchaseWiseAdditionalChargesDto
    //{
    //    public int ProductIds { get; set; } = Array.Empty<int>();
    //    public IList<ChargeData> ChargeData { get; set; }
    //}
}
