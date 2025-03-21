namespace POSV1.TenantAPI.Startup
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
    }
}
