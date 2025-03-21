using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Estimate
{
    public partial class est02estimatesalesitems : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int est02uin { get; set; }
        public int est02estimatesalesuin { get; set; }
        public int est02pro02uin { get; set; }
        public double est02qty { get; set; }
        public int est02un01uin { get; set; }
        public decimal est02rate { get; set; }
        public double est02sub_total { get; set; }
        public decimal est02disc_amt { get; set; }
        public double est02net_amt { get; set; }
        public string? est02ref_no { get; set; }
        public int? est02emp01uin { get; set; }
        public int? est02vec02uin { get; set; }
        public double? est02vatper { get; set; }
        public decimal? est02transportationfee { get; set; }
        public string? est02destination { get; set; }
        public virtual pro02products pro02products { get; set; }
        public virtual est02estimatesales est02estimatesales { get; set; }
        public virtual un01units un01units { get; set; }
    }
}
