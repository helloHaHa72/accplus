using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantAPI.Models
{
    public partial class VMVendorWithTxnDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Registration_No { get; set; }
        public string Tel_No { get; set; }
        public decimal? Opening_Balance { get; set; }
        public DateTime Registered_Date { get; set; }
        public string Ledger_code { get; set; }
        public IList<VMVenPurchase> Purchases { get; set; }
    }

    public partial class VMVenPurchase
    {
        public int Id { get; set; }
        [NotMapped]
        public DateTime _date { get; set; }
        public string Date => _date.ToString("yyyy/MM/dd");
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Invoice_No { get; set; }
        public decimal Net_Amt { get; set; }
        public string Products {  get; set; }
        public string Remarks { get; set; }
    }

    public partial class VMVenFuelLogBook
    {
        [NotMapped]
        public DateTime _date { get; set; }
        public string Date => _date.ToString("yyyy/MM/dd");
        public string Time => _date.ToString("hh/mm tt");
        public int PumpId { get; set; }
        public string PumpName { get; set; }
        public decimal Quantity { get; set; }
        public string Token_No { get; set; }
        public string Remarks { get; set; }
    }
}
