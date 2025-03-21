using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.EntityModels.Production
{
    public class add03purchaseadditionalchargesdetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int add03uin {  get; set; }
        public string add03title { get; set; } = null!;
        public decimal add03amount { get; set; }
        public string? add03remarks { get; set; }
        public int add02uin { get; set; }
        // Navigation property for the related add02purchaseadditionalcharges entity
        public add02purchaseadditionalcharges PurchaseAdditionalCharges { get; set; } = null!;
    }
}
