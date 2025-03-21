using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class pur01purchasereturns : Auditable
    {
        public pur01purchasereturns()
        {
            this.pur02returnitems = new HashSet<pur02returnitems>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pur01returnuin { get; set; } // Unique identifier for purchase return
        public int pur01uin { get; set; } // Reference to original purchase
        public string pur01return_date_nep { get; set; } // Nepali date for the return
        public DateTime pur01return_date { get; set; } // English date for the return
        public string pur01return_invoice_no { get; set; } // Return invoice number
        public int pur01ven01uin { get; set; } // Vendor reference ID
        public string pur01return_remarks { get; set; } // Remarks for the return
        public decimal pur01return_sub_total { get; set; } // Subtotal for the return
        public double pur01return_vat_per { get; set; } // VAT percentage for the return
        public decimal pur01return_vat_amt { get; set; } // VAT amount for the return
        public decimal pur01return_total { get; set; } // Total amount for the return
        public decimal pur01return_disc_amt { get; set; } // Discount amount for the return
        public decimal pur01return_additional_disc { get; set; } // Additional discount
        public decimal pur01return_net_amt { get; set; } // Net amount for the return
        public bool pur01return_status { get; set; } // Status of the return
        public bool pur01return_is_deleted { get; set; } // Soft delete flag
        public string? pur01returnvoucher_no { get; set; }
        // Navigation properties
        public virtual ven01vendors ven01vendors { get; set; } // Vendor details
        public virtual pur01purchases pur01purchase { get; set; } // Reference to the original purchase
        public virtual ICollection<pur02returnitems> pur02returnitems { get; set; } // Collection of return items
    }
}
