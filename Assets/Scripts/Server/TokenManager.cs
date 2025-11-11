using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Server
{
    public static class TokenManager
    {
        private const string PlayerPrefsKey = GlobalConstants.PlayerPrefsKeys.Token;
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("16ByteSecretKey!"); // 16 bytes for AES

        public static string GetToken()
        {
            var token = PlayerPrefs.HasKey(PlayerPrefsKey)
                ? PlayerPrefs.GetString(PlayerPrefsKey)
                : CreateToken();

            return token;
        }
        
        private static string CreateToken()
        {
            var token = Guid.NewGuid().ToString();
            var encrypted = EncryptToken(token);
            
            PlayerPrefs.SetString(PlayerPrefsKey, encrypted);
            PlayerPrefs.Save();
            
            return encrypted;
        }

        private static string EncryptToken(string token)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = new byte[16]; // zero IV for simplicity

            using var encryptor = aes.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(token);
            var encrypted = encryptor.TransformFinalBlock(input, 0, input.Length);

            return Convert.ToBase64String(encrypted);
        }
    }
}