using BaseAppSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class tran01transaction_types : Auditable
	{
        public tran01transaction_types()
        {
            tran02transaction_summaries = new HashSet<tran02transaction_summaries>();
        }

        [Key]
		public int tran01uin { get; set; }

		public string tran02name { get; set; }

		public virtual ICollection<tran02transaction_summaries> tran02transaction_summaries { get; set; }

    }
}