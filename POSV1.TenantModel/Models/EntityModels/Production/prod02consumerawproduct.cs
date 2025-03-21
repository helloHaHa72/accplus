using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseAppSettings;

namespace POSV1.TenantModel.Models.EntityModels.Production
{
    public class prod02consumerawproduct : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int prod02uin {  get; set; }
        public int prod02productionuin { get; set; }
        public int prod02productuin { get; set; }
        public string prod2productname { get; set; } = null!;
        public int prod02unituin { get; set; }
        public string prod02unitname { get; set; } = null!;
        public decimal prod02rate { get; set; }
        public double prod02qty { get; set; }
        public bool prod02isallused {  get; set; }
        public double? prod02remainingqty { get; set; }
        public virtual prod01production Production { get; set; }
        public virtual pro02products Product { get; set; }
    }
}
