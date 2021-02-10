using Assets.Scripts.CustomPlugins.Utility;
using System;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Contains group of security functions.
/// </summary>
public class Security
{
    /// <summary>
    /// The key used in algorithm.
    /// </summary>
    private const string p = "5^@n$:Z4EnUB9fx)";

    /// <summary>
    /// The salt used in algorithm. 
    /// </summary>
    private const string s = "9a1f59f5e7f5fd82cd23927e456cefd1";

    /// <summary>
    /// The initialization vector used in algorithm.
    /// </summary>
    private const string v = "dc522bd1629ca858";

    /// <summary>
    /// AES encryption.
    /// </summary>
    /// <param name="plainText">Text to encrypt.</param>
    /// <returns>Returns encrypted text.</returns>
    public static string Encrypt(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] keyBytes = new Rfc2898DeriveBytes(p, Encoding.ASCII.GetBytes(s)).GetBytes(256 / 8);
        using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
        {
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(v));
            byte[] cipherTextBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
    }

    /// <summary>
    /// AES decryptor.
    /// </summary>
    /// <param name="encryptedText">Text to decrypt.</param>
    /// <returns>Returns decrypted string.</returns>
    public static string Decrypt(string encryptedText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        int decryptedByteCount = 0;

        byte[] keyBytes = new Rfc2898DeriveBytes(p, Encoding.ASCII.GetBytes(s)).GetBytes(256 / 8);
        using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None })
        {
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(v));
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
        }

        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
    }

    /// <summary>
    /// Just simple encoding to give minimum protection against cheaters and attempts to modify
    /// high score. Playfab has some issues with custom javascript libraries for RSA :I.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <returns>Encoded string.</returns>
    public static string MagicHat(string value)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }

    /// <summary>
    /// Validates <see cref="RegisterData"/>.
    /// </summary>
    /// <param name="data">The data to validate.</param>
    /// <param name="success">On success perform this action.</param>
    /// <param name="failed">On failed perform this action.</param>
    public static void ValidateData(RegisterData data, Action success, Action<string> failed)
    {
        if(data.Password != data.PasswordRepeated)
        {
            failed(Const.VALIDATION_PASSWORD_MISMATCH);
            return;
        }

        if(data.Password.Length <= Const.VALIDATION_PASSWORD_LENGHT)
        {
            failed(Const.VALIDATION_PASSWORD_SHORT);
            return;
        }
        
        if(!IsEmailValid(data.Email))
        {
            failed(Const.VALIDATION_INVALID_EMAIL);
            return;
        }

        if(data.Username.Length < Const.VALIDATION_USERNAME_LENGHT)
        {
            failed(Const.VALIDATION_USERNAME_SHORT);
            return;
        }

        success();
    }

    /// <summary>
    /// Checks if email is valid. Avoiding mess with using REGEX.
    /// </summary>
    /// <param name="emailaddress">The email address to check.</param>
    /// <returns></returns>
    public static bool IsEmailValid(string emailaddress)
    {
        try
        {
            var m = new MailAddress(emailaddress);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
