using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace BaseAppSettings
{
    public class SerilogRecords
    {
        public SerilogRecords() { }
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }


    }
    public class ApiLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ApplicationName { get; set; } = "";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string HttpMethod { get; set; } = "";
        public string Path { get; set; } = "";
        public string RequestBody { get; set; } = "";
        public string RequestHeaders { get; set; } = "";
        public int ResponseStatusCode { get; set; } = 200;
        public string ResponseHeaders { get; set; } = "";
        public string ResponseBody { get; set; } = "";
    }
    public static class AppStartup
    {
    }
}