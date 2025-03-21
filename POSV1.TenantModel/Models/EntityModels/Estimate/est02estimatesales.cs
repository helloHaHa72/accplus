using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Estimate
{
    public partial class est02estimatesales : Auditable
    {
        public est02estimatesales()
        {
            est02estimatesalesitems = new HashSet<est02estimatesalesitems>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int est02uin { get; set; }
        public string est02date_nep { get; set; }
        public DateTime est02date_eng { get; set; }
        public int est02est01uin { get; set; }
        public int? est02cus01uin { get; set; }
        public string est02invoice_no { get; set; }
        public string est02remarks { get; set; }
        public decimal est02sub_total { get; set; }
        public decimal est02disc_amt { get; set; }
        public decimal est02disc_percentage { get; set; }
        public decimal est02total { get; set; }
        public bool est02status { get; set; }
        public bool est02deleted { get; set; }
        public string? est02voucher_no { get; set; }
        //[MaxLength(25)]
        //public string BranchCode { get; set; } = null!;
        public virtual cus01customers? cus01customers { get; set; }
        public virtual ICollection<est02estimatesalesitems> est02estimatesalesitems { get; set; }
        //public virtual OrgBranch BranchData { get; set; } = null!;
        public virtual est01estimate est01estimate { get; set; } = null!;
    }
}
