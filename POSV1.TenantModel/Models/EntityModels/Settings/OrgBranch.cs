using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Estimate;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantModel.Models.EntityModels.Settings
{
    public class OrgBranch : Auditable
    {
        [Key]
        [MaxLength(25)]
        public string BranchCode { get; set; } = null!;
        [MaxLength(255)]
        public string BranchName { get; set; } = null!;
        public bool IsDefault { get; set; } = false;
        public virtual ICollection<pur01purchases> Purchases { get; set; } = new HashSet<pur01purchases>();
        public virtual ICollection<sal01sales> Sale { get; set; } = new HashSet<sal01sales>();
        //public virtual ICollection<est02estimatesales> EstimateSale { get; set; } = new HashSet<est02estimatesales>();
        public virtual ICollection<cus01customers> Customer { get; set; } = new HashSet<cus01customers>();
        public virtual ICollection<ven01vendors> Vendor { get; set; } = new HashSet<ven01vendors>();
        public virtual ICollection<UserBranch> UserBranches { get; set; } = new HashSet<UserBranch>();
    }
}
