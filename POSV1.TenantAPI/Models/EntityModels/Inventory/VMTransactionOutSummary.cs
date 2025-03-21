namespace POSV1.TenantAPI.Models
{
    public partial class VMTransactionOutSummary
    {
        public int? ID { get; set; }
        public int TransactionTypeID { get; set; }
        public int? SupplierID { get; set; }
        public int? CustomerID { get; set; }
        public string Invoice_No { get; set; }
        public string Bill_No { get; set; }
        public decimal Taxable_Amt { get; set; }
        public float Tax_Percent { get; set; }
        public decimal Tax_Amt { get; set; }
        public decimal Sub_Total { get; set; }
        public string Remarks { get; set; }
        public decimal Discount_Amt { get; set; }
        public decimal Discount_Percent { get; set; }
        public decimal Net_Amt { get; set; }
        public bool Status { get; set; }
        public IList<VMTransactionOutDetails> VMTransactionOutDetails { get; set; }
    }

    public partial class VMTransactionOutDetails
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int SaleID { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public float Quantity { get; set; }
        public float Base_Quantity { get; set; }
        public bool Taxable { get; set; }
        public float Tax_Percent { get; set; }
        public decimal Tax_Amt { get;set; }
        public decimal Sub_Total { get; set;}
        public decimal Discount_Amt { get; set; }
        public decimal Discount_Percent { get;set; }
        public decimal Net_Amt { get; set;}
        public bool Status { get; set; }

    }
}

