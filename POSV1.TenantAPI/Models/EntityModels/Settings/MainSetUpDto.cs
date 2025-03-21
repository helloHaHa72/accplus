using System.IO.Pipelines;

namespace POSV1.TenantAPI.Models.EntityModels.Settings
{
    public class MainSetUpDto
    {
    }

    public class MainSetupCreateDto
    {
        public string Email { get; set; }//will act as defualt username as well as email also, should be unique
        public string Password { get; set; }
        public string DBUser { get; set; }
        public string OrgName { get; set; } = null!;
        //public string Logo { get; set; }
        public string HostName { get; set; } = null!;
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string DbPassword { get; set; }
    }

    public class MainSetupUpdateDto
    {
        public string OrgName { get; set; } = null!;
        //public string Logo { get; set; }
        public string HostName { get; set; } = null!;
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string DbPassword { get; set; }
    }

    public class MainSetupViewDto
    {
        public int Id { get; set; }
        public string OrgName { get; set; } = null!;
        //public string Logo { get; set; }
        public string HostName { get; set; } = null!;
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string DBUser { get; set; }
    }
}
