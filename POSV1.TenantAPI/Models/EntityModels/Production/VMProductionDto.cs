using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantAPI.Models.EntityModels.Production
{
    public class VMProductionDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<VMConsumerRawProduct> RawProducts { get; set; } = new();
    }

    public class VMConsumerRawProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int UnitId { get; set; }
        public string UnitName { get; set; } = null!;
        public decimal Rate { get; set; }
        public double Qty { get; set; }
        //public bool prod02isallused { get; set; }
        //public double? prod02remainingqty { get; set; }
    }

    public class ViewProductionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Description { get; set; }
        public string Status {  get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<ViewConsumerRawProduct> RawProducts { get; set; } = new();
        public List<ViewFinalProduct> FinalProducts { get; set; } = new();
    }

    public class ViewConsumerRawProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int UnitId { get; set; }
        public string UnitName { get; set; } = null!;
        public decimal Rate { get; set; }
        public double Qty { get; set; }
        public double? RemainingQty { get; set; }
        //public bool prod02isallused { get; set; }
        //public double? prod02remainingqty { get; set; }
    }

    public class VMFinalProduct
    {
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public string Description {  get; set; }
        public int Qty {  get; set; }
        public DateTime Date {  get; set; }
        public string Remarks { get; set; }
    }

    public class ViewFinalProduct : VMFinalProduct
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal Ratio {  get; set; }
    }
}
