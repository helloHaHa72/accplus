using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Estimate;
using POSV1.TenantModel.Models.EntityModels.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{

    public partial class cus01customers : Auditable
    {
        public cus01customers()
        {
            sal01sales = new HashSet<sal01sales>();
            est02estimatesales = new HashSet<est02estimatesales>();
            tran02transaction_summaries = new HashSet<tran02transaction_summaries>();
            //this.inv02_2sales_meta = new HashSet<inv02_2sales_meta>();
            this.CashSettlements = new HashSet<cas01cashsettlement>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cus01uin { get; set; }

        //public int cus01bra01uin { get; set; }
        public string cus01led_code { get; set; }
        public string cus01name_eng { get; set; }
        public string cus01name_nep { get; set; }
        public string cus01address { get; set; }
        public string cus01tel { get; set; }
        public decimal cus01opening_bal { get; set; }
        public string cus01reg_no { get; set; }
        public bool cus01status { get; set; }
        public bool cus01deleted { get; set; }
        public int? cus01customerTypeId { get; set; }
        public System.DateTime cus01registered_date { get; set; }
        public bool cus01isvat { get; set; }
        //public string cus010number { get; set; }
        public string BranchCode { get; set; } = null!;

        //public virtual bra01branch bra01branch { get; set; }
        public virtual ICollection<sal01sales> sal01sales { get; set; }
        public virtual ICollection<est02estimatesales> est02estimatesales { get; set; }
        public virtual ICollection<tran02transaction_summaries> tran02transaction_summaries { get; set; }
        public virtual cus02customerType? CustomerType { get; set; }
        public virtual ICollection<cas01cashsettlement> CashSettlements { get; set; }
        public virtual OrgBranch BranchData { get; set; } = null!;
    }
}
