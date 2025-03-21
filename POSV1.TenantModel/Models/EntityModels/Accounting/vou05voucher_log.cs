using BaseAppSettings;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantModel.Models
{
    public class vou05voucher_log : Auditable
    {
        [Key]
        public int vou05uin { get; set; }
        public string vou05vou02uin { get; set; }
        public EnumVoucherStatus voucherStatus { get; set; }
    }
}
