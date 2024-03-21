using LearnFrameworkApi.Api.Helpers;
using LearnFrameworkApi.Module.Helpers.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace LearnFrameworkApi.Api.Controllers.Learn
{
    [Route("api/learn/[controller]")]
    [ApiController]
    public class HelperTestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public HelperTestController(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }

        [HttpGet("Env")]
        public IActionResult Env()
        {
            var result = new
            {
                Domain = EnvironmentVariableHelper.StripeDomain(_configuration),
                Username = EnvironmentVariableHelper.StripeUsername(_configuration),
                Port = EnvironmentVariableHelper.StripePort(_configuration),
            };
            return Ok(result);
        }

        [HttpGet("Hash")]
        public IActionResult Hash()
        {
            string password = "admin";
            string passwordMD5 = HashingHelper.GetMd5Hash(password);

            string secretKey = "protel";
            string token = secretKey + DateTime.Now.ToString("yyyyMMdd");
            string tokenHash256 = HashingHelper.GetMd5Hash(token);

            var result = new
            {
                password,
                passwordMD5,
                token,
                tokenHash256
            };
            return Ok(result);
        }

        [HttpGet("Encode")]
        public IActionResult Encode()
        {
            string text = "admin";
            string textEncodeBase64 = EncodingHelper.Base64Encode(text);
            string textDecodeBase64 = EncodingHelper.Base64Decode(textEncodeBase64);

            var result = new
            {
                text,
                textEncodeBase64,
                textDecodeBase64
            };
            return Ok(result);
        }

        [HttpGet("AesEncryption")]
        public IActionResult AesEncryption()
        {
            var plainText = "Hai beb";
            var key = "verySecretKey1234567891234567891";
            var ciphertext = EncryptionHelper.AesEncryptString(key, plainText);
            var plainTextReceived = EncryptionHelper.AesDecryptString(key, ciphertext);

            var result = new
            {
                plainText,
                key,
                ciphertext,
                plainTextReceived
            };
            return Ok(result);
        }

        [HttpGet("RsaEncryption")]
        public IActionResult RsaEncryption()
        {
            string original = "secret message";
            string encrypted;
            string decrypted;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                encrypted = EncryptionHelper.EncryptString(original, rsa.ExportParameters(false), false);
                decrypted = EncryptionHelper.DecryptString(encrypted, rsa.ExportParameters(true), false);
            }

            var result = new
            {
                original, 
                encrypted, 
                decrypted
            };
            return Ok(result);
        }
    }
}
