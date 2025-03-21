using System;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantModel.Models
{
    public partial class cfg01configurations
    {
        [Key]
        public int cfg01uin { get; set; }
        public string cfg01module { get; set; }
        public string cfg01key { get; set; }
        public string cfg01value { get; set; }
        public DateTime cfg01created_date { get; set; }
        public string cfg01created_name { get; set; }
        public string cfg01updated_name { get; set; }
        public DateTime cfg01updated_date { get; set; }
    }
}
