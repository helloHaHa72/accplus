using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantModel.Models.EntityModels.Inventory
{
    public partial class sup01suppliers : Auditable
    {
        public sup01suppliers()
        {
            tran02transaction_summaries = new HashSet<tran02transaction_summaries>();
        }
        [Key]
        public int sup01uin { get; set; }

        [MaxLength(255)]
        public string sup01address { get; set; }
        [MaxLength(25)]
        public string sup01tel { get; set; }
        public DateTime? sup01regDate { get; set; }
        [MaxLength(25)]
        public string sup01regNo { get; set; }
        public decimal sup01balance { get; set; }
        public virtual ICollection<tran02transaction_summaries> tran02transaction_summaries { get; set; }
    }
}
