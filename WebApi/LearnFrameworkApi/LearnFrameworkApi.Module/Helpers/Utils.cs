using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers
{
    public static class Utils
    {
        public static string ActiveToString(this bool active)
        {
            if (active) return "Active";
            else return "Not Active";
        }
        public static string GenerateRandomNumberAsString(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static int GenerateRandomNumber(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return int.Parse(result);
        }
    }
}
