using BaseAppSettings;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Production
{
    public class prod04finalproducts : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int prod04uin {  get; set; }
        public int prod4productionuin { get; set; }
        public int prod04productuin { get; set; }
        public string prod04productname { get; set; } = null!;
        public int prod04unituin { get; set; }
        public string prod04unitname { get; set; } = null!;
        public decimal prod04unitratio { get; set; }
        public string? prod04desc { get; set; }
        public int prod04qty { get; set; }
        public DateTimeOffset prod04date { get; set; }
        public string? prod04remarks { get; set; }
        public virtual prod01production Production { get; set; } = null!;
        public virtual pro02products Product { get; set; } = null!;
        public virtual un01units Unit { get; set; } = null!;
        public virtual ICollection<prod04finalproducts> FinalProducts { get; set; } = new HashSet<prod04finalproducts>();

    }
}
