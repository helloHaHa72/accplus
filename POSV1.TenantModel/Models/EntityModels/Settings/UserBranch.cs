using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.EntityModels.Settings
{
    public class UserBranch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        //[ForeignKey(nameof(UserId))]
        //public virtual IdentityUser User { get; set; } = null!;

        [Required]
        public string BranchCode { get; set; } = null!;

        [ForeignKey(nameof(BranchCode))]
        public virtual OrgBranch Branch { get; set; } = null!;
    }
}
