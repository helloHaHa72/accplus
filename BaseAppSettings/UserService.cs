namespace BaseAppSettings;

public static class EncryptionService
{
    private static readonly string encryptionKey = "YourSecureKeyHere";

    public static void SaveUserPassword(int userId, string plainPassword)
    {
        string encryptedPassword = EncryptionHelper.Encrypt(plainPassword, encryptionKey);
        // Code to save encryptedPassword to database against userId
    }

    public static string RetrieveUserPassword(int userId)
    {
        // Code to get encryptedPassword from database using userId
        string encryptedPassword = "RetrievedEncryptedPasswordFromDB";
        return EncryptionHelper.Decrypt(encryptedPassword, encryptionKey);
    }
}
