using BaseAppSettings;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class pur02items: Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pur02uin { get; set; }
        public int pur02pur01uin { get; set; }
        public int pur02pro02uin { get; set; }
        public decimal pur02rate { get; set; }
        public int pur02qty { get; set; }
        public int pur02un01uin { get; set; }
        public decimal? pur02amount { get; set; }
        public DateTime? pur02mfg_date { get; set; }
        public DateTime? pur02exp_date { get; set; }
        public string? pur02batch_no { get; set; }
        public decimal pur02disc_amt { get; set; }
        public decimal pur02net_amt { get; set; }
        public virtual pro02products pro02products { get; set; }
        public virtual pur01purchases pur01purchases { get; set; }
        public virtual un01units un01units { get; set; }
    }
}
