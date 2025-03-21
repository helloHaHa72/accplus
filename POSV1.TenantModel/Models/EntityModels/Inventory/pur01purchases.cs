using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.CloudR2;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Models.EntityModels.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class pur01purchases : Auditable
    {
        public pur01purchases()
        {
            this.pur02items = new HashSet<pur02items>();
            this.CashSettlements = new HashSet<cas01cashsettlement>();
            this.ChargePurchaseRelations = new HashSet<add04chargepurchaserel>();
            this.CloudR2Purchase = new HashSet<CloudR2Purchase>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pur01uin { get; set; }
        public string pur01date_nep { get; set; }
        public DateTime pur01date { get; set; }
        public string pur01invoice_no { get; set; }
        public int pur01ven01uin { get; set; }
        public string pur01remarks { get; set; }
        public decimal pur01sub_total { get; set; }
        public double pur01vat_per { get; set; }
        public decimal pur01vat_amt { get; set; }
        public decimal pur01total { get; set; }
        public decimal pur01additionalcharge { get; set; }
        public bool pur01status { get; set; }
        public bool pur01is_deleted { get; set; }
        public decimal pur01disc_amt { get; set; }
        public decimal pur01disc_percentage { get; set; }
        public decimal pur01additional_disc { get; set; }
        public decimal pur01net_amt { get; set; }
        public bool pur01vatapplicable { get; set; }
        public bool pur01vatclamable { get; set; }
        public decimal? pur01tdspercentage {  get; set; }
        public EnumPaymentStatus pur01payment_status { get; set; } = EnumPaymentStatus.FullCredit;
        public string? pur01voucher_no {  get; set; }
        public string BranchCode { get; set; } = null!;
        public virtual ven01vendors ven01vendors { get; set; }
        public virtual ICollection<pur02items> pur02items { get; set; }
        public virtual ICollection<pur01purchasereturns> pur01purchasereturns { get; set; }
        public virtual ICollection<cas01cashsettlement> CashSettlements { get; set; }
        public virtual OrgBranch BranchData { get; set; } = null!;
        public virtual ICollection<add04chargepurchaserel> ChargePurchaseRelations { get; set; }
        public virtual ICollection<CloudR2Purchase> CloudR2Purchase { get; set; }

    }
}
