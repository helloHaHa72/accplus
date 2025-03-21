namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class VMPurchaseReturn
    {
        public int Id { get; set; }
        public int VendorID { get; set; }
        public int PurchaseId { get; set; }
        //public string VendorName { get; set; }
        public DateTime Return_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        //public decimal Sub_Total { get; set; }
        //public decimal Disc_Amt { get; set; }
        //public decimal Additional_Disc_Amt { get; set; }
        //public decimal Total { get; set; }
        //public decimal VAT_Per { get; set; }
        //public decimal VAT_Amt { get; set; }
        //public decimal Net_Amt { get; set; }

        public List<VMCreatePurchaseReturnItemDetails> VMPurchaseReturnItems { get; set; } = new List<VMCreatePurchaseReturnItemDetails>();
    }

    public class VMPurchaseReturnList
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime Date { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Additional_Disc_Amt { get; set; }
        public decimal Total { get; set; }
        public decimal VAT_Per { get; set; }
        public decimal VAT_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
    }

    public class VMPurchaseReturnDetail
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime Date { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Additional_Disc_Amt { get; set; }
        public decimal Total { get; set; }
        public decimal VAT_Per { get; set; }
        public decimal VAT_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
        public string Net_Amt_Words { get; set; }
        public List<VMPurchaseReturnItemDetails> VMPurchaseReturnItemDetails { get; set; } = new List<VMPurchaseReturnItemDetails>();
    }

    public class VMPurchaseReturnItemDetails
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string HsCode { get; set; }
        public double Ratio { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Rate { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Mgf_Date { get; set; }
        public DateTime? Exp_Date { get; set; }
        public string? Batch_No { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Net_Amt { get; set; }
    }

    public class VMCreatePurchaseReturnItemDetails
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        //public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        //public string UnitName { get; set; }
        public decimal Rate { get; set; }
        //public decimal Amount { get; set; }
        public DateTime? Mgf_Date { get; set; }
        public DateTime? Exp_Date { get; set; }
        public string? Batch_No { get; set; }
        public decimal Disc_Amt { get; set; }
        //public decimal Net_Amt { get; set; }
    }
}
