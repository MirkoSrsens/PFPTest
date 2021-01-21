using Assets.Scripts.CustomPlugins.Utility;
using System;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;


public class Security
{
    static readonly string p = "5^@n$:Z4EnUB9fx)";
    static readonly string s = "9a1f59f5e7f5fd82cd23927e456cefd1";
    static readonly string v = "dc522bd1629ca858";

    public static string Encrypt(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] keyBytes = new Rfc2898DeriveBytes(p, Encoding.ASCII.GetBytes(s)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
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

    public static string Decrypt(string encryptedText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = new Rfc2898DeriveBytes(p, Encoding.ASCII.GetBytes(s)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

        var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(v));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
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

    public static void ValidateData(RegisterData data, Action success, Action<string> failed)
    {
        if(data.Password != data.PasswordRepeated)
        {
            failed("Passwords needs to match");
            return;
        }

        if(data.Password.Length < 6)
        {
            failed("Password needs to be at least 6 letters long");
            return;
        }
        
        if(!IsEmailValid(data.Email))
        {
            failed("Not valid email!");
            return;
        }

        if(data.Username.Length < 5)
        {
            failed("Username must have more than 5 letters");
            return;
        }

        success();
    }

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
