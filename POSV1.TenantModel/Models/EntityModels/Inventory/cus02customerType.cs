using BaseAppSettings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public class cus02customerType : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cus02Id { get; set; }
        public string cus02Name { get; set; } = null!;
        public double? cus02DiscountPercenatge { get; set; }
    }
}
