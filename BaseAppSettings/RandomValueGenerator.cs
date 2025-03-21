namespace BaseAppSettings;

public class RandomValueGenerator
{
    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
    public static string GenerateRandomPassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        var random = new Random();
        var password = new char[length];
        var charIndex = 0;

        // Generate at least one character of each type
        password[charIndex++] = chars[random.Next(26)]; // Random uppercase letter
        password[charIndex++] = chars[random.Next(26, 52)]; // Random lowercase letter
        password[charIndex++] = chars[random.Next(52, 62)]; // Random numeric digit
        password[charIndex++] = chars[random.Next(62, chars.Length)]; // Random symbol

        // Generate remaining characters
        for (; charIndex < length; charIndex++)
        {
            password[charIndex] = chars[random.Next(chars.Length)];
        }

        // Shuffle the password characters
        for (var i = 0; i < length; i++)
        {
            var j = random.Next(length);
            var temp = password[i];
            password[i] = password[j];
            password[j] = temp;
        }

        return new string(password);
    }


}
