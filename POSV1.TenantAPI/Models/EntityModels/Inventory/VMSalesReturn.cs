namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class VMSalesReturn
    {
        //public int Id { get; set; }
        public int CustomerId { get; set; }
        //public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public int SaleId {  get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        //public decimal Sub_Total { get; set; }
        //public decimal Disc_Amt { get; set; }
        //public decimal Total { get; set; }
        //public double VAT_Per { get; set; }
        //public decimal VAT_Amt { get; set; }
        //public decimal Net_Amt { get; set; }
        public List<VMSalesReturnItem> VMSalesReturnItem { get; set; }
    }

    public class VMSalesDetailWithReturnDetails
    {
        public VMSaleDetail VMSaleDetail { get; set; }
        public IList<VMSalesReturnDetail> VMSalesReturnDetail { get; set; }
        public IList<GroupedReturnItem> GroupedReturnItem { get; set; }
    }

    public class VMSalesReturnList
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public string Invoice_No { get; set; }
        public int SaleId { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Total { get; set; }
        public double VAT_Per { get; set; }
        public decimal VAT_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
    }

    public class VMSalesReturnDetail
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public string Invoice_No { get; set; }
        public int SaleId { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Total { get; set; }
        public double VAT_Per { get; set; }
        public decimal VAT_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public string Net_Amt_Words {  get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
        public List<VMSalesReturnItemDetails> VMSalesReturnItemDetails { get; set; }
    }

    public class VMSalesReturnItem
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        //public string ProductName { get; set; }
        public int UnitID { get; set; }
        //public string UnitName { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        //public double subTotal { get; set; }
        //public double netAmt { get; set; }
    }

    public class VMSalesReturnItemDetails
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string HsCode { get; set; }
        public double Ratio { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public double Quantity { get; set; }
        public decimal Rate { get; set; }
        public double Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public double Net_Amt { get; set; }
    }

}
