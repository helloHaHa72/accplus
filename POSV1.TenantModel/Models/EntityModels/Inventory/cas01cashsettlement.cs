using BaseAppSettings;
using System;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public class cas01cashsettlement : Auditable
    {
        //few of the fields wont be use now but later can be used for the purchase/sales wise transaction
        public int cas01uin {  get; set; }
        public int? cas01purchaseuin { get; set; }
        public int? cas01saleuin { get; set; }
        public int? cas01customeruin { get; set; }
        public int? cas01vendoruin { get; set; }
        public string? cas01invoice_no { get; set; }
        public EnumPaymentStatus? cas01payment_status { get; set; }
        public EnumPaymentType cas01payment_type { get; set; }
        public decimal cas01amount { get; set; }
        public string? cas01remarks { get; set; }
        public bool cas01isbank {  get; set; }
        public string? cas01bank_ledname { get; set; }
        public string? cas01chqnumber {  get; set; }
        public decimal? cas01tdspercentage { get; set; }
        public DateTime cas01transaction_date { get; set; }
        public string? cas0101voucher_no { get; set; }
        public virtual pur01purchases pur01purchases { get; set; }
        public virtual sal01sales sal01sales { get; set; }
        public virtual cus01customers cus01customers { get; set; }
        public virtual ven01vendors ven01vendors { get; set; }
    }
}
