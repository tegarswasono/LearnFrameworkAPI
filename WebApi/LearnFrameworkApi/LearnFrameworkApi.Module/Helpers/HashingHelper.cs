using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers
{
    //https://digitalsains.blogspot.com/2016/08/perbedaan-encoding-encryption-hashing.html
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
            string yyyyMMdd = DateTime.Now.ToString("yyyyMMdd");
            string stringCombination = secretKey + yyyyMMdd;
            byte[] byteOfStringCombination = Encoding.UTF8.GetBytes(stringCombination);

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
