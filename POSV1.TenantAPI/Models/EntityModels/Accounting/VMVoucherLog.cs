using System.ComponentModel.DataAnnotations.Schema;
using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMVoucherLog
    {
        public int Id { get; set; }
        public string VoucherId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset _createdDate { get; set; }
        //public string CreatedDate => _createdDate.ToString("yyyy-MM-dd");
        public string CreatedBy { get; set; }
    }
}
