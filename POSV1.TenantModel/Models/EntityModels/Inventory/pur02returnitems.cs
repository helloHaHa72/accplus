using BaseAppSettings;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class pur02returnitems : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pur02returnuin { get; set; } // Unique identifier for return items
        public int pur02returnpur01uin { get; set; } // Foreign key to purchase return
        public int pur02returnpro02uin { get; set; } // Foreign key to product
        public decimal pur02returnrate { get; set; } // Rate for the returned item
        public int pur02returnqty { get; set; } // Quantity of returned item
        public int pur02returnun01uin { get; set; } // Foreign key to unit
        public decimal? pur02returnamount { get; set; } // Amount for the returned item
        public DateTime? pur02returnmfg_date { get; set; } // Manufacturing date
        public DateTime? pur02returnexp_date { get; set; } // Expiry date
        public string? pur02returnbatch_no { get; set; } // Batch number of the returned item
        public decimal pur02returndisc_amt { get; set; } // Discount amount on the returned item
        public decimal pur02net_amt { get; set; } // Net amount for the returned item

        // Navigation properties
        public virtual pro02products pro02products { get; set; } // Reference to product details
        public virtual pur01purchasereturns pur01purchasereturns { get; set; } // Reference to purchase return
        public virtual un01units un01units { get; set; } // Reference to unit details
    }
}
