using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class sal01salesreturn : Auditable
    {
        public sal01salesreturn()
        {
            this.sal02itemsreturn = new HashSet<sal02itemsreturn>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sal01uin { get; set; }
        public int sal01_salId { get; set; }
        public string sal01date_nep { get; set; }
        public DateTime sal01date_eng { get; set; }
        public int sal01cus01uin { get; set; }
        public string sal01invoice_no { get; set; }
        public string sal01remarks { get; set; }
        public decimal sal01sub_total { get; set; }
        public decimal sal01disc_amt { get; set; }
        public decimal sal01total { get; set; }
        public double sal01vat_per { get; set; }
        public decimal sal01vat_amt { get; set; }
        public decimal sal01net_amt { get; set; }
        public bool sal01status { get; set; }
        public bool sal01deleted { get; set; }
        public string? sal01returnvoucher_no { get; set; }
        public virtual cus01customers cus01customers { get; set; }
        public virtual ICollection<sal02itemsreturn> sal02itemsreturn { get; set; }
        public virtual sal01sales sal01sales { get; set; }
    }
}
