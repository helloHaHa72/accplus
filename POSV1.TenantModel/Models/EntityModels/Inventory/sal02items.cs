using BaseAppSettings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class sal02items : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sal02uin { get; set; }
        public int sal02sal01uin { get; set; }
        public int sal02pro02uin { get; set; }
        public double sal02qty { get; set; }
        public int sal02un01uin { get; set; }
        public decimal sal02rate { get; set; }
        public double sal02sub_total { get; set; }
        public decimal sal02disc_amt { get; set; }
        public double sal02net_amt { get; set; }
        public string? sal02ref_no { get; set; }
        public int? sal02emp01uin { get; set; }
        public int? sal02vec02uin { get; set; }
        public double? sal02vatper { get; set; }
        public decimal? sal02transportationfee { get; set; }
        public string? sal02destination { get; set; }
        public virtual pro02products pro02products { get; set; }
        public virtual sal01sales sal01sales { get; set; }
        public virtual un01units un01units { get; set; }
    }
}
