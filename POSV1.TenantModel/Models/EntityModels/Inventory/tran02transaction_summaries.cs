using BaseAppSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class tran02transaction_summaries : Auditable
    {
        public tran02transaction_summaries()
        {
            tran04transaction_out_details = new HashSet<tran04transaction_out_details>();
        }

        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int tran02uin { get; set; }

		//[ForeignKey("tran01transaction_types")]
		public int tran02tran01uin { get; set; }
		public virtual tran01transaction_types tran01transaction_types { get; set; }

		//[ForeignKey("sup01suppliers")]
		public int? tran02sup01uin { get; set; }
		public virtual sup01suppliers sup01suppliers { get; set; }

		//[ForeignKey("cus01customers")]
		public int? tran02cus01uin { get; set; }
		public virtual cus01customers cus01customers { get; set; }

		public string tran02invoice_no { get; set; }

		public string tran02bill_no { get; set; }

		public decimal tran02taxable_amount { get; set; }

		public float tran02tax_percent { get; set; }

		public decimal tran02tax_amount { get; set;}

		public decimal tran02sub_total { get; set; }

		public string tran02remark { get; set; }

		public decimal tran02discount_amount { get; set; }

		public float tran02discount_percent { get;set; }

		public bool tran02discount_type { get;set; }

		public decimal tran02net_amount { get; set; }

		public string tran02status { get; set; }

		public System.DateTime tran02transaction_date { get; set; }

		public System.DateTime tran02created_date { get; set; }

		public string tran02updated_name { get; set; }

		public string tran02created_name { get; set; }	

		public System.DateTime tran02updated_date { get; set; }

		public virtual ICollection<tran04transaction_out_details> tran04transaction_out_details { get; set; }

	}
}