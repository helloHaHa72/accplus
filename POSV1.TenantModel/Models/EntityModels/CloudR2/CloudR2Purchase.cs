using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.EntityModels.CloudR2
{
    public class CloudR2Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public JobProcessingEnum ProcStatus { get; set; }
        public string? FailRemarks { get; set; }
        public string? Path { get; set; }
        public string? CloudR2Path { get; set; }
        public pur01purchases Purchase { get; set; }
    }
}
