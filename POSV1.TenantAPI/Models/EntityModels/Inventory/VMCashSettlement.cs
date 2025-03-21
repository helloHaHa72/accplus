using POSV1.TenantModel;

namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class VMCashSettlement
    {
        public int Id { get; set; }
        public int? PurchaseId { get; set; }
        public int? SaleId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
    }


    public class InvoiceList
    {
        public int PurchaseId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        //public decimal TDsPercentage { get; set; }
        //public bool IsTDsApplied { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public double VatPercentage { get; set; }
        public decimal VatAmount { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class VMCashSettlementCustomerWise
    {
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public decimal Amount { get; set; }
        public bool IsBank { get; set; }
        public string BankName { get; set; }
        public string ChqNumber { get; set; }
        public string Remarks { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsBillPayment { get; set; }
        public int? PurchaseId { get; set; }
        public decimal? Tds { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
    }

    public class CreateCashSettlement
    {
        public int PurchaseId { get; set; }
        public int SaleId { get; set; }
        public string InvoiceNumber { get; set; }
        //public EnumPaymentStatus PaymentStatus { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateCashSettlementCustomerWise
    {
        public int VendorId { get; set; }
        public int CustomerId { get; set; }
        public string Remarks { get; set; }
        public DateTime Date {  get; set; }
        //public string InvoiceNumber { get; set; }
        //public EnumPaymentStatus PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public bool IsBank { get; set; }
        public string BankName {  get; set; }
        public string ChqNumber { get; set; }
        public bool IsBillPayment { get; set; }
        //public string InvoiceNumber { get; set; }
        public IList<int> PurchaseId {  get; set; }
        public decimal? Tds { get; set; }
    }

    public class UpdateCashSettlementCustomerWise : CreateCashSettlementCustomerWise
    {
        public int Id { get; set; }
    }
}
