using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers.Security
{
    public static class NetsPaymentHelper
    {
        public static string GenerateSignature(string txnReq, string secretKey)
        {
            string concatPayloadAndSecretKey = txnReq + secretKey;
            return EncodeBase64(HashSHA256ToBytes(Encoding.UTF8.GetBytes(concatPayloadAndSecretKey)));
        }
        public static byte[] HashSHA256ToBytes(byte[] data)
        {
            byte[] bytes = SHA256.HashData(data);
            return bytes;
        }
        public static string EncodeBase64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }
}
