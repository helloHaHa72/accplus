using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Estimate;
using POSV1.TenantModel.Models.EntityModels.Production;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class pro02products : Auditable
    {
        public pro02products()
        {
            pro03units = new HashSet<pro03units>();
            sal02items = new HashSet<sal02items>();
            est02estimatesalesitems = new HashSet<est02estimatesalesitems>();
            pur02items = new HashSet<pur02items>();
            tran04transaction_out_details = new HashSet<tran04transaction_out_details>();
            RawProducts = new HashSet<prod02consumerawproduct>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int pro02uin { get; set; }
        public string pro02code { get; set; }
        public string pro02hscode { get; set; }
        public string pro02name_eng { get; set; }
        public string pro02name_nep { get; set; }
        public int pro02pro01uin { get; set; }
        public string pro02description { get; set; }
        public int? pro02un01uin { get; set; }
        public decimal pro02last_cp { get; set; }
        public decimal pro02last_sp { get; set; }
        public double pro02opening_qty { get; set; }
        public bool pro02status { get; set; }
        public bool pro02enable_stock { get; set; }
        public bool pro02is_taxable { get; set; }
        public bool pro02hasmultipleunits { get; set; } = false;
        public string pro02image_url { get; set; }
        public virtual pro01categories pro01categories { get; set; }
        public virtual un01units un01units { get; set; }
        public virtual ICollection<pro03units> pro03units { get; set; }
        public virtual ICollection<sal02items> sal02items { get; set; }
        public virtual ICollection<est02estimatesalesitems> est02estimatesalesitems { get; set; }
        public virtual ICollection<pur02items> pur02items { get; set; }
        public virtual ICollection<tran04transaction_out_details> tran04transaction_out_details { get; set; }
        public virtual ICollection<prod02consumerawproduct> RawProducts { get; set; }
        public virtual ICollection<prod04finalproducts> FinalProducts { get; set; } = new HashSet<prod04finalproducts>();
    }
}
