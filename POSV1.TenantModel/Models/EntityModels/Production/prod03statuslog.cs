using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseAppSettings;

namespace POSV1.TenantModel.Models.EntityModels.Production
{
    public class prod03statuslog : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int prod03uin { get; set; }
        public EnumProductionStatus prod03previousstatus { get; set; }
        public EnumProductionStatus prod03newstatus { get; set; }
        public string? prod03remarks { get; set; }
        public int prod03productionuin { get; set; }
        public virtual prod01production Production { get; set; } = null!;
    }
}
