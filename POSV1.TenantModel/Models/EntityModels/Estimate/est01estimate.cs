using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.EntityModels.Estimate
{
    public class est01estimate : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int est01uin { get; set; }
        public string? est01customername { get; set; }
        public string est01refnumber { get; set; } = null!;
        public EnumEstimateStatus est01status { get; set; }
        public virtual ICollection<est02estimatesales> est02estimatesales { get; set; }
    }
}
