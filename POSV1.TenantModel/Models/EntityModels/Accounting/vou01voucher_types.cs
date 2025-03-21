using BaseAppSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models
{
    public partial class vou01voucher_types : Auditable
    {
        public vou01voucher_types()
        {
            this.vou02voucher_summary = new HashSet<vou02voucher_summary>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int vou01uin { get; set; }
        public string vou01title { get; set; }
        public int vou01last_no { get; set; }
        public string vou01prefix { get; set; }
        public virtual ICollection<vou02voucher_summary> vou02voucher_summary { get; set; }
    }
}
