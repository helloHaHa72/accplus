namespace POSV1.TenantAPI.Models.EntityModels
{
    public class VMEestimate
    {
        public int? CustomerId { get; set; }
        //public string RefNumber { get; set; } = string.Empty;
        //public string Invoice_No { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        //public string DateNepali { get; set; } = string.Empty; // If you need Nepali date format
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public decimal Total { get; set; }
        public List<VMEestimateItem> VMEestimateItem { get; set; } = new List<VMEestimateItem>();
    }

    public class VMEestimateItem
    {
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public double Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public bool IsVatApplied { get; set; }
        public int DriverId { get; set; }
        public int VechileId { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string Chalan_No { get; set; } = string.Empty;
        public decimal Transportation_Fee { get; set; }
    }

    public class EstimateList
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string RefNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate {  get; set; }
    }

    public class EstimateDetailDto
    {
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string RefNumber { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Total { get; set; }
        public List<EstimateItemDto> EstimateItems { get; set; } = new List<EstimateItemDto>();
    }

    public class EstimateItemDto
    {
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public double Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public int? DriverId { get; set; }
        public int? VehicleId { get; set; }
        public string Destination { get; set; }
        public string ChalanNumber { get; set; }
        public decimal TransportationFee { get; set; }
        public bool IsVatApplied { get; set; }
    }

}
