using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models
{
    public partial class vou03voucher_details : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int vou03uin { get; set; }
       
        public string vou03vou02full_no { get; set; }
        public int vou03led05uin { get; set; }
        public decimal vou03dr { get; set; }
        public decimal vou03cr { get; set; }
        public string vou03description { get; set; }
        public string vou03chq { get; set; }
        public decimal vou03balance { get; set; }

        public virtual led01ledgers led01ledgers { get; set; }
        public virtual vou02voucher_summary vou02voucher_summary { get; set; }
    }
}
