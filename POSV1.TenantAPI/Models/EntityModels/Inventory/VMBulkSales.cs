namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class VMBulkSaleInformation
    {
        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
        public IEnumerable<VMBulkSale> Sales { get; set; }
    }

    public class VMBulkSale : VMBulkSaleItem
    {
        public int CustomerId { get; set; }
        public bool IsVatApplied { get; set; }
        public string Invoice_No { get; set; }
        public decimal Sub_Total => (decimal)netAmt;
        public decimal Total => Sub_Total;
        public decimal VatPercent => 0;
        public decimal VAT_Amt => Total * (VatPercent * (decimal)0.01);
        public decimal Net_Amt => Total + VAT_Amt;
        //public IEnumerable<VMBulkSaleItem> SaleItems { get; set; }
    }

    public class VMBulkSaleItem
    {
        public int ProductID { get; set; }
        public double Quantity { get; set; }
        public int UnitID { get; set; }
        public decimal Rate { get; set; }
        public double subTotal => Quantity * (double)Rate;
        public double netAmt => subTotal;
        public int? DriverId { get; set; }
        public int? VechileId { get; set; }
        public string DestinationAddress { get; set; }
        public string Chalan_Number { get; set; }
        public decimal? TransactionFee { get; set; }
    }
}
