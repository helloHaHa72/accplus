using BaseAppSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Production
{
    public class add02purchaseadditionalcharges : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int add02uin {  get; set; }
        public ICollection<add03purchaseadditionalchargesdetail> AdditionalChargesDetails { get; set; } = new List<add03purchaseadditionalchargesdetail>();
        public virtual ICollection<add04chargepurchaserel> ChargePurchaseRelations { get; set; } = new List<add04chargepurchaserel>();
    }
}
