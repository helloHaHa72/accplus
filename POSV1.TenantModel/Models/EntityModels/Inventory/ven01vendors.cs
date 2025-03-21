using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class ven01vendors : Auditable
    {
        public ven01vendors()
        {
            this.pur01purchases = new HashSet<pur01purchases>();
            this.CashSettlements = new HashSet<cas01cashsettlement>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ven01uin { get; set; }
        public string ven01led_code { get; set; }
        public string ven01name_eng { get; set; }
        public string ven01name_nep { get; set; }
        public string ven01address { get; set; }
        public string ven01reg_no { get; set; }
        public string ven01tel { get; set; }
        public decimal? ven01opening_bal { get; set; }
        public bool ven01status { get; set; }
        public bool ven01is_deleted { get; set; }
        public bool ven01isvat { get; set; }
        //public string ven010number {  get; set; }
        public string BranchCode { get; set; } = null!;
        public DateTime ven01registered_date { get; set; }
        public virtual ICollection<pur01purchases> pur01purchases { get; set; }
        public virtual ICollection<cas01cashsettlement> CashSettlements { get; set; }
        public virtual OrgBranch BranchData { get; set; } = null!;
    }
}
