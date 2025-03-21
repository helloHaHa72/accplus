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
    public class prod01production : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int prod01uin { get; set; }
        public string prod01title { get; set; } = null!;
        public string prod01code {  get; set; } = null!;
        public string? prod01description { get; set; }
        public EnumProductionStatus prod01status { get; set; }
        public DateTimeOffset prod01startdate { get; set; }
        public DateTimeOffset? prod01enddate { get; set; }
        public virtual ICollection<prod02consumerawproduct> RawProducts { get; set; } = new HashSet<prod02consumerawproduct>();
        public virtual ICollection<prod03statuslog> StatusLogs { get; set; } = new HashSet<prod03statuslog>();
        public virtual ICollection<prod04finalproducts> FinalProducts { get; set; } = new HashSet<prod04finalproducts>();
    }
}
