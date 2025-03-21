using System.Security.Cryptography;

namespace BaseAppSettings;
public class EncryptionHelper
{
    private const int KeySize = 256;
    private const int SaltSize = 16;
    private const int Iterations = 10000;

    public static string Encrypt(string plainText, string key)
    {
        byte[] salt = GenerateSalt();
        using (var aes = Aes.Create())
        {
            aes.KeySize = KeySize;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var keyDerivation = new Rfc2898DeriveBytes(key, salt, Iterations, HashAlgorithmName.SHA256))
            {
                aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
                aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);
            }

            using (var encryptor = aes.CreateEncryptor())
            using (var ms = new MemoryStream())
            {
                ms.Write(salt, 0, salt.Length);
                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string encryptedText, string key)
    {
        byte[] cipherData = Convert.FromBase64String(encryptedText);
        byte[] salt = new byte[SaltSize];
        Array.Copy(cipherData, 0, salt, 0, SaltSize);

        using (var aes = Aes.Create())
        {
            aes.KeySize = KeySize;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var keyDerivation = new Rfc2898DeriveBytes(key, salt, Iterations, HashAlgorithmName.SHA256))
            {
                aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
                aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);
            }

            using (var decryptor = aes.CreateDecryptor())
            using (var ms = new MemoryStream(cipherData, SaltSize, cipherData.Length - SaltSize))
            using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cryptoStream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltSize];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}
