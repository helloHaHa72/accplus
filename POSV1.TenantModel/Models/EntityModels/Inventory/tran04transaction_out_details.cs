using BaseAppSettings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class tran04transaction_out_details: Auditable
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int tran04uin { get; set; }

		[ForeignKey("tran02transaction_summaries")]
		public int tran04tran02uin { get; set; }
		public virtual tran02transaction_summaries tran02transaction_summaries { get; set; }

		[ForeignKey("pro02products")]
		public int tran04pro02uin { get; set; }
		public virtual pro02products pro02products { get; set; }

        [ForeignKey("sal01sales")]
        public int tran04sal01uin { get; set; }
		public virtual sal01sales sal01sales { get; set; }

		public string tran04unit { get; set; }

		public decimal tran04unit_price { get; set; }

		public float tran04quantity { get; set; }

		public float tran04base_quantity { get; set; }

		public bool tran04taxable { get; set; }

		public float tran04tax_percent { get; set; }

		public decimal tran04tax_amount { get; set; }

		public decimal tran04sub_total { get; set; }

		public decimal tran04discount_amount { get; set; }

		public float tran04discount_percent { get; set; }

		public bool tran04discount_type { get; set; }

		public decimal tran04net_amount { get; set; }

		public string tran04status { get; set; }


	}
}