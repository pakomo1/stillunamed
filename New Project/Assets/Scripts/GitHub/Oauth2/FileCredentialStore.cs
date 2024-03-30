using Octokit;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Application = UnityEngine.Application;

public class FileCredentialStore
{
    private static FileCredentialStore instance;
    private readonly string filePath;
    private string encryptionKey;

    private FileCredentialStore(string fileName, string key)
    {
        this.filePath = Path.Combine(Application.persistentDataPath, fileName); ;
        this.encryptionKey = key;
    }

    public static FileCredentialStore Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FileCredentialStore("accessToken.txt", "thisisasecretkeyforaesencryption");
            }
            return instance;
        }
    }


    public async Task<Credentials> GetAccessToken()
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        var encryptedJson = await File.ReadAllTextAsync(filePath);
        var decryptedJson = DecryptString(encryptedJson);
        var accessTokenResponse = JsonUtility.FromJson<AccessTokenResponse>(decryptedJson);

        return new Credentials(accessTokenResponse.access_token);
    }

    public async Task SaveCredentials(string accessToken, string tokenType, string scope)
    {
        var accessTokenResponse = new AccessTokenResponse
        {
            access_token = accessToken,
            token_type = tokenType,
            scope = scope
        };
        var json = JsonUtility.ToJson(accessTokenResponse);
        var encryptedJson = EncryptString(json);

        await File.WriteAllTextAsync(filePath, encryptedJson);
    }

    public string EncryptString(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16]; // Initialization vector for AES, it can be any 16 bytes

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public string DecryptString(string encryptedText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16]; // Initialization vector for AES, it can be any 16 bytes

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
