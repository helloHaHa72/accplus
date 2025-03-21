namespace POSV1.TenantAPI.Extensions
{
    public static class DateTimeExtension
    {

        public static string DateConverter(this DateTime dateTime )
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
