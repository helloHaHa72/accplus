namespace POSV1.TenantAPI.Models.EntityModels.ERP
{
    public class VatReportDto
    {
        public List<PurchaseDto> PurchaseDetails { get; set; }
        public List<SalesDto> SalesDetails { get; set; }
        public NonClaimableVatDto NonClaimableVatDetails { get; set; }
        public VatSummaryDto Summary { get; set; }
    }

    public class PurchaseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNo { get; set; }
        public string VendorName { get; set; }
        public int VendorId { get; set; }
        public string VatPan {  get; set; }
        public decimal TotalAmount { get; set; }
        public bool VatApplicable { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmountWithVat { get; set; }
        public bool VatClaimable { get; set; }
    }

    public class SalesDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string VatPan { get; set; }
        public decimal TotalAmount { get; set; }
        public bool VatApplicable { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmountWithVat { get; set; }
        public bool VatClaimable { get; set; }
    }

    public class NonClaimableVatDto
    {
        public List<NonClaimableDto> NonClaimablePurchases { get; set; }
        public List<NonClaimableDto> NonClaimableSales { get; set; }
    }

    public class NonClaimableDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmountWithVat { get; set; }
    }

    public class VatSummaryDto
    {
        public decimal TotalSalesVat { get; set; }
        public decimal TotalPurchaseVat { get; set; }
        public decimal TotalNonClaimableVat { get; set; }
        public decimal NetPayableVat { get; set; }
    }

    public class ItemVatSummaryDto
    {
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int TransactionCount { get; set; }
        public decimal PayableAmount { get; set; }  // From sales
        public decimal ReceivableAmount { get; set; }  // From purchases
        public decimal PayableVat { get; set; }  // Sales VAT
        public decimal ReceivableVat { get; set; }  // Purchase VAT
    }
}
