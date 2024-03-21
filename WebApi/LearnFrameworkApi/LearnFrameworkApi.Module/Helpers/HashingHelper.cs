using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers
{
    public static class HashingHelper
    {
        //Most Popular Hash algoritm: MD5, SHA256, SHA512
        public static string GetMd5Hash(string input)
        {
            string result = "";
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                result = sBuilder.ToString();
            }
            return result;
        }
        public static string GetSHA256Token(string secretKey)
        {
            byte[] byteOfStringCombination = Encoding.UTF8.GetBytes(secretKey);
            using (var hashEngine = SHA256.Create())
            {
                var hashedBytes = hashEngine.ComputeHash(byteOfStringCombination, 0, byteOfStringCombination.Length);
                var sb = new StringBuilder();
                foreach (var b in hashedBytes)
                {
                    var hex = b.ToString("x2");
                    sb.Append(hex);
                }
                return sb.ToString();
            }
        }
    }
}
