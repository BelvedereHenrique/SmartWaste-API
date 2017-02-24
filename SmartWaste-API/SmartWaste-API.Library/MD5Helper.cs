using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Library
{
    public static class MD5Helper
    {
        public static bool Check(string hash, string value)
        {
            var md5 = MD5.Create();
            var byteArray = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(value));

            var hash2 = byteArray.Aggregate(new StringBuilder(), (s, b) => s.Append(b.ToString("X2"))).ToString();

            return hash.ToLower() == hash2.ToLower();
        }
        public static string Create(string data) {
            var md5 = MD5.Create();
            var byteArray = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(data));
            var hash = byteArray.Aggregate(new StringBuilder(), (s, b) => s.Append(b.ToString("X2"))).ToString();
            return hash;
        }
    }
}
