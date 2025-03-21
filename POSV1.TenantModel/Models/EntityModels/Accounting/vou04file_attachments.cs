using BaseAppSettings;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models
{
    public class vou04file_attachments : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int vou04uin { get; set; }
        
        public string vou04vou02full_no { get; set; }
        
        public string vou04filename { get; set; }
        
        public string vou04location { get; set; }
        public bool vou04deleted { get; set; }
       
        public string vou04created_name { get; set; }
        public string vou04updated_name { get; set; }
        public DateTime vou04created_date { get; set; }
        public DateTime vou04updated_date { get; set; }
        public virtual vou02voucher_summary vou02voucher_summary { get; set; }
    }
}
