using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.Common
{
    public static class Algorithm
    {
        /// <summary>
        /// 将字符串转换成md5 (大写)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToMD5(string source)
        {
            using (MD5 md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = md5.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }
        public static string ToMD52(string source)
        {
            MD5 md5 = MD5.Create();

            var bytes = Encoding.UTF8.GetBytes(source);
            var hashBytes = md5.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

        }
        public static string ToMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder(32);
                foreach (var item in hashBytes)
                {
                    sb.Append(item.ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
