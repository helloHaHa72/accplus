using System;
using System.ComponentModel.DataAnnotations.Schema;
using BaseAppSettings;
using POSV1.TenantModel.Modules;

namespace POSV1.TenantModel.Models
{
    public class Setting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public EnumModules modules { get; set; }
        public string values { get; set; }
        public DateTime lastUpdated_date { get; set; }
        public string lastUpdated_by { get; set; }
    }
}
