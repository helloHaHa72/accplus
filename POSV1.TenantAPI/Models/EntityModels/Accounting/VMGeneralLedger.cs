using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantAPI.Models
{
    public class VMGeneralLedger
    {
        public int ID { get; set; }
        public  string GLName { get; set; }
        public  string Code { get; set; }
        public string GLedgerNameCode { get; set; }
        public  GLType GLType { get; set; }
        public  string GlTypeName { get; set; }
        public  string Description { get; set; }
        public  int? ParentGLID { get; set; }
        public  string? ParentGLName { get; set; }
        public  bool Status { get; set; }
    }

    public class VMCreateGeneralLedger
    {
        public string GLName { get; set; }
        public string Code { get; set; }
        public GLType GLType { get; set; }
        public string Description { get; set; }
        public int? ParentGLID { get; set; }
        public bool Status { get; set; }
    }

    public class LedgerTypeDto
    {
        public int Led05Uin { get; set; }
        public string Led05Title { get; set; } = string.Empty;
        public string Led05TitleNep { get; set; } = string.Empty;
        public bool Led05AddDr { get; set; }
        public string CreatedName { get; set; } = string.Empty;
        public string UpdatedName { get; set; } = string.Empty;
        public string DeletedName { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.MinValue;
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
    }

}
