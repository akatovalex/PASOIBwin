using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;

namespace PASOIBwin {
    public static class Encrypting {
        public static byte[] ToAes256(byte[] data, byte[] key) {
            Aes aes = Aes.Create();
            aes.GenerateIV();
            aes.Key = key;
            byte[] encrypted;
            ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, crypt, CryptoStreamMode.Write)) 
                    using (BinaryWriter writer = new BinaryWriter(cryptoStream))
                        writer.Write(data);
                encrypted = memoryStream.ToArray();
            }
            return encrypted.Concat(aes.IV).ToArray();
        }
        public static byte[] FromAes256(byte[] encryptedData, byte[] key) {
            byte[] bytesIv = new byte[16];
            byte[] mess = new byte[encryptedData.Length - 16];
            for (int i = encryptedData.Length - 16, j = 0; i < encryptedData.Length; i++, j++)
                bytesIv[j] = encryptedData[i];
            for (int i = 0; i < encryptedData.Length - 16; i++)
                mess[i] = encryptedData[i];
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = bytesIv;
            byte[] result;
            ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream(mess)) {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, crypt, CryptoStreamMode.Read)) 
                    using (BinaryReader reader = new BinaryReader(cryptoStream))
                        result = reader.ReadBytes(mess.Length);
            }
            return result;
        }
    }
}
