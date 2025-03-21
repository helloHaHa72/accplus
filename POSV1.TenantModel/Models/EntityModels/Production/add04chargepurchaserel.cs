using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.EntityModels.Production
{
    public class add04chargepurchaserel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int add04uin { get; set; }
        public int add04puraddchargeuin { get; set; }
        public int add04purchaseuin { get; set; }
        public bool add04isdeleted { get; set; } = false;
        public virtual add02purchaseadditionalcharges PurchaseAdditionalCharges { get; set; } = null!;
        public virtual pur01purchases Purchase { get; set; } = null!;
    }
}
