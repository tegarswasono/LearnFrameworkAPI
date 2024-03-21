using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers
{
    //Most Popular Encryption: AES, RSA, and DES
    /*
     plainText = text before encryption
     cipherText = text after encryption

    AES = sending and receive message using same key
    RSA = sending message using public key and receive using private key
    DES = same like aes, but beat by aes, this one is old algoritm
     */
    public static class EncryptionHelper
    {
        //#01 Aes Encryption Algoritm need Key with 32 character
        public static string AesEncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string AesDecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        //#02 RSA 
        public static string EncryptString(string inputString, RSAParameters parameters, bool fOAEP)
        {
            byte[] encryptedData;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(parameters);

                encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(inputString), fOAEP);
            }

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptString(string inputString, RSAParameters parameters, bool fOAEP)
        {
            byte[] decryptedData;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(parameters);

                decryptedData = rsa.Decrypt(Convert.FromBase64String(inputString), fOAEP);
            }

            return Encoding.UTF8.GetString(decryptedData);
        }


    }
}
