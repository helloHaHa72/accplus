using System.Globalization;

namespace POSV1.TenantAPI.Utility
{
    public class GeneralUtility
    {
        public static string GenerateRandomSixCharacterCode()
        {
            var random = new Random();

            // Generate three random digits
            string digits = random.Next(1000, 9999).ToString();

            char letter1 = (char)random.Next('A', 'Z' + 1);
            char letter2 = (char)random.Next('A', 'Z' + 1);

            var code = new List<char>(digits + letter1 + letter2);
            code = code.OrderBy(_ => random.Next()).ToList();

            return new string(code.ToArray());
        }

        public static string GenerateProductionCode(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            // Take first 4 letters, ensuring it's at least 4 characters
            string prefix = new string(title
                .Trim()
                .Replace(" ", "") // Remove spaces
                .Take(4)
                .ToArray())
                .ToUpper(); // Ensure uppercase

            // Get current date in yyMMdd format
            string datePart = DateTime.UtcNow.ToString("yyMMdd", CultureInfo.InvariantCulture);

            // Generate 3 random uppercase letters
            Random random = new Random();
            string randomLetters = new string(Enumerable.Range(0, 3)
                .Select(_ => (char)random.Next('A', 'Z' + 1))
                .ToArray());

            // Combine parts with hyphens
            return $"{prefix}-{datePart}-{randomLetters}";
        }

        public static List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                errors.Add("PasswordTooShort");

            if (!password.Any(char.IsUpper))
                errors.Add("PasswordRequiresUpper");

            if (!password.Any(char.IsDigit))
                errors.Add("PasswordRequiresDigit");

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                errors.Add("PasswordRequiresNonAlphanumeric");

            return errors;
        }

        public static string GenerateReferenceNumber()
        {
            string datePart = DateTime.UtcNow.ToString("yyyy-MMdd"); // Format YYYY-MMDD
            string randomString = GenerateRandomString(4); // Generate 4-character random string

            return $"{datePart}-{randomString}";
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
