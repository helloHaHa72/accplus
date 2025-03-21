using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Estimate;
using POSV1.TenantModel.Models.EntityModels.Production;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class un01units : Auditable
    {
        public un01units()
        {
            pro02products = new HashSet<pro02products>();
            pur02items = new HashSet<pur02items>();
            pro03units = new HashSet<pro03units>();
            sal02items = new HashSet<sal02items>();
            est02estimatesalesitems = new HashSet<est02estimatesalesitems>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int un01uin { get; set; }
        public string un01name_eng { get; set; }
        public string un01name_nep { get; set; }
        public string? un01desc { get; set; }
        public double un01ratio { get; set; }
        public bool un01status { get; set; }
        public virtual ICollection<sal02items> sal02items { get; set; }
        public virtual ICollection<est02estimatesalesitems> est02estimatesalesitems { get; set; }
        public virtual ICollection<pro02products> pro02products { get; set; }
        public virtual ICollection<pro03units> pro03units { get; set; }
        public virtual ICollection<pur02items> pur02items { get; set; }
        public virtual ICollection<prod04finalproducts> FinalProducts { get; set; } = new HashSet<prod04finalproducts>();

    }
}
