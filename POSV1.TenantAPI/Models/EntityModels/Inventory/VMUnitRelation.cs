namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class VMUnitRelation
    {
        public int UnitId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public double Ratio { get; set; }
        //public bool Status { get; set; }
        public bool IsDefault {  get; set; }
    }

    public class VMViewUnit
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public double Ratio { get; set; }
        //public bool Status { get; set; }
        public bool IsDefault { get; set; }
    }
}
