using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{

    public partial class sal01sales :Auditable
    {
        public sal01sales()
        {
            this.sal02items = new HashSet<sal02items>();
            this.tran04transaction_out_details = new HashSet<tran04transaction_out_details>();
            this.sal01salesreturns = new HashSet<sal01salesreturn>();
            this.CashSettlements = new HashSet<cas01cashsettlement>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int sal01uin { get; set; }
        public string sal01date_nep { get; set; }
        public System.DateTime sal01date_eng { get; set; }
        public int sal01cus01uin { get; set; }
        public string sal01invoice_no { get; set; }
        public string sal01remarks { get; set; }
        public decimal sal01sub_total { get; set; }
        public decimal sal01disc_amt { get; set; }
        public decimal sal01disc_percentage { get; set; }
        public decimal sal01total { get; set; }
        public double sal01vat_per { get; set; }
        public decimal sal01vat_amt { get; set; }
        public decimal sal01net_amt { get; set; }
        public bool sal01status { get; set; }
        public bool sal01deleted { get; set; }
        public bool sal01vatapplicable { get; set; }
        public bool sal01vatclamable { get; set; }
        public string? sal01voucher_no { get; set; }
        public string BranchCode { get; set; } = null!;
        public EnumPaymentStatus sal01payment_status { get; set; } = EnumPaymentStatus.FullCredit;
        public virtual cus01customers cus01customers { get; set; }
        public virtual ICollection<sal02items> sal02items { get; set; }
        public virtual ICollection<tran04transaction_out_details> tran04transaction_out_details { get; set; }
        public virtual ICollection<sal01salesreturn> sal01salesreturns { get; set; }
        public virtual ICollection<cas01cashsettlement> CashSettlements { get; set; }
        public virtual OrgBranch BranchData { get; set; } = null!;
    }
}
