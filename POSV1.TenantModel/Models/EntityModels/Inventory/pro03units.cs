using BaseAppSettings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{


    public partial class pro03units : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int pro03uin { get; set; }
        public int pro03pro02uin { get; set; }
        public int pro03un01uin { get; set; }
        public double pro03ratio { get; set; }
        public decimal pro03last_cp { get; set; }
        public decimal pro03last_sp { get; set; }
        public bool pro03status { get; set; }
        public bool IsDefault { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public virtual pro02products pro02products { get; set; }
        public virtual un01units un01units { get; set; }

    }
}
