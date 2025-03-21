using BaseAppSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{

    public partial class pro01categories : Auditable
    {
        public pro01categories()
        {
            pro02products = new HashSet<pro02products>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int pro01uin { get; set; }
        public string pro01code { get; set; }
        public string pro01name_eng { get; set; }
        public string pro01name_nep { get; set; }
        public bool pro01status { get; set; }
        public virtual ICollection<pro02products> pro02products { get; set; }
    }
}
