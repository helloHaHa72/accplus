namespace POSV1.TenantAPI.Models.EntityModels.ERP
{
    public class TaxSettlementCreateDto
    {
        public string Title { get; set; } = null!;
        public decimal TaxPercentage { get; set; }
    }

    public class TaxSettlementUpdateDto
    {
        public string Title { get; set; } = null!;
        public decimal TaxPercentage { get; set; }
    }

    public class TaxSettlementViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal TaxPercentage { get; set; }
        public bool CanBeDeleted { get; set; }
    }

    public class ViewSystemTax
    {
        public decimal TaxPercentage { get; set; }
        public string IsTaxActive {  get; set; }
    }
}
