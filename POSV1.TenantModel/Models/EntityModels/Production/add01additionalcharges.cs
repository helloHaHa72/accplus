using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using BaseAppSettings;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.EntityModels.Production
{
    public class add01additionalcharges : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int add01uin { get; set; }
        public string add01title { get; set; } = null!;
        public string? add01description { get; set; }
    }
}
