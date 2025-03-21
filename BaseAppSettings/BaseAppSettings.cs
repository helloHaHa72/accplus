using System.Text;

namespace BaseAppSettings
{
    public static class ExceptionExtension
    {

        public static string DetailErrorLog(this Exception ex)
        {
            if (ex.InnerException == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);

                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.InnerException.Message);
                return sb.ToString();
            }
        }
    }

    public class ConnectIpsConfig
    {
        public int MERCHANTID { get; set; }
        public string APPID { get; set; } = null!;
        public string APPNAME { get; set; } = null!;
        public string TXNCRNCY { get; set; } = null!;
        public string pfxPass { get; set; } = null!;
    }

    public class FacebookConfig
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }

    public class GeneralAppConfig
    {
        public bool IsUATMode { get; set; }
        public bool LogHTTPRequests { get; set; }
        public bool EnableHangFireJobs { get; set; }
        public bool EnableClourR2BatchProcessing { get; set; }
        public bool SaveLogs { get; set; }
        public string ResetPasswordCustomer { get; set; }
        public string ResetPasswordAdmin { get; set; }
        public string EmailVerification { get; set; }
        public string FrontEndURL { get; set; } = "https://www.qrmenu.asia";
    }
    public class GoogleDriveConfig
    {
        public string AppName { get; set; }
        public string ServiceAccJSONFile { get; set; }
        public string RootFolder { get; set; }
        public string PublicViewURL { get; set; }
        public string GenerateFileURL(string id, int maxWidth = 1000) => string.Format(PublicViewURL, id, maxWidth);
    }

    public class ImageConfig
    {
        public int Size { get; set; }
    }

    public class XceltextSMSConfig
    {
        public string BaseURL { get; set; }
        public string ApiKey { get; set; }
        public string SenderId { get; set; }
    }

    public static class BaseAppSettings
    {
        public static bool IsUATMode => GeneralAppConfig.IsUATMode;
        public static bool LogHttpRequests => GeneralAppConfig.LogHTTPRequests;
        public static GeneralAppConfig GeneralAppConfig { get; set; } = new GeneralAppConfig();
        public static GoogleDriveConfig GoogleDriveConfig { get; set; } = new GoogleDriveConfig();
        public static ImageConfig ImageConfig { get; set; } = new ImageConfig();
        public static XceltextSMSConfig XceltextSMSConfig { get; set; } = new XceltextSMSConfig();
    }

}