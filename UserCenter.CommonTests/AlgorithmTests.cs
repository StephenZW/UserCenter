using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserCenter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UserCenter.Common.Tests
{
    [TestClass()]
    public class AlgorithmTests
    {
        [TestMethod()]
        public void ToMd5Test()
        {
            var source = "将字符串转换成md5 (大写)";
            Stopwatch s = Stopwatch.StartNew();
            for (int i = 0; i < 5000_000; i++)
            {
                var str = Algorithm.ToMD5(source);
            }
            s.Stop();
            //ToMD5 : 00:00:32.8736872
            Console.WriteLine(nameof(Algorithm.ToMD5) + " : " + s.Elapsed);
        }

        [TestMethod()]
        public void ToMd5Test2()
        {
            var source = "将字符串转换成md5 (大写)";
            Stopwatch s = Stopwatch.StartNew();
            for (int i = 0; i < 5000_000; i++)
            {
                var str = Algorithm.ToMD52(source);
            }
            s.Stop();
            //ToMD52 : 00:00:30.1910658
            Console.WriteLine(nameof(Algorithm.ToMD52) + " : " + s.Elapsed);
        }
    }
}