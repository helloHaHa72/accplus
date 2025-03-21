using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models
{
    public partial class vou02voucher_summary : Auditable
    {
        public vou02voucher_summary()
        {
            this.vou03voucher_details = new HashSet<vou03voucher_details>();
            this.vou04file_attachments = new HashSet<vou04file_attachments>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string vou02full_no { get; set; }
        public int vou02number { get; set; }
        public int vou02vou01uin { get; set; }
        public decimal vou02amount { get; set; }
        public string vou02description { get; set; }
        public string vou02chq { get; set; }
        public Nullable<int> vou02contra_led05uin { get; set; }
        public string vou02manual_vno { get; set; }
        public bool vou02deleted { get; set; }
        public bool vou02is_approved { get; set; }
        public bool vou02is_sys_generated { get; set; }
        public DateTime vou02value_date { get; set; }
        public EnumVoucherStatus vou02status { get; set; }
        public virtual led01ledgers led01ledgers { get; set; }
        public virtual vou01voucher_types vou01voucher_types { get; set; }
        public virtual ICollection<vou03voucher_details> vou03voucher_details { get; set; }
        public virtual ICollection<vou04file_attachments> vou04file_attachments { get; set; }
    }
}
