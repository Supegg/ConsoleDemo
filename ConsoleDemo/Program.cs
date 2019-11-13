using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");
            Console.WriteLine("CommandLine: {0}", Environment.CommandLine);
            //Console.WriteLine(args[0]);
            //DateTime t = DateTime.Now;
            //Console.WriteLine("{0},{1}", t, t.Ticks);

            //Random r = new Random();
            //double d;

            //do
            //{
            //    d = r.Next(-82000, 82000);
            //    try
            //    {
            //        Console.WriteLine(calY(d));
            //    }
            //    catch
            //    {
            //    }
            //    System.Threading.Thread.Sleep(1000);
            //} while (true);

            //new StaticTest().DoTest();

            testDatetime();

            Console.Read();

        }

        static void testDatetime()
        {
            DateTime now = DateTime.Now;
            DateTime utcNow = DateTime.UtcNow;

            Console.WriteLine("now:\t{0} ticks", now.Ticks);
            Console.WriteLine("utc now:{0} ticks", utcNow.Ticks);
            Console.WriteLine("now diff:{0} ticks", (now - utcNow).Ticks);

            DateTime epoch0 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            DateTime epoch1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTime epoch2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            Console.WriteLine("{0}\t{1}\t{2}", epoch0.ToLocalTime(), epoch1.ToLocalTime(), epoch2.ToLocalTime());

            Console.WriteLine("{0}\t{1}\t{2}", epoch0.Ticks, epoch1.Ticks, epoch2.Ticks); // they are all equal. why?

            Console.WriteLine("now0:\t\t{0} ms", (now - epoch0).TotalMilliseconds);
            Console.WriteLine("now1:\t\t{0} ms", (now - epoch1).TotalMilliseconds);
            Console.WriteLine("now2:\t\t{0} ms", (now - epoch2).TotalMilliseconds);

            //unix-like ms timestamp
            Console.WriteLine("utc now0:\t{0} ms", (utcNow - epoch0).TotalMilliseconds);
            Console.WriteLine("utc now1:\t{0} ms", (utcNow - epoch1).TotalMilliseconds);
            Console.WriteLine("utc now2:\t{0} ms", (utcNow - epoch2).TotalMilliseconds);

            //Console.WriteLine("epoch diff:{0} ms", (epoch0 - epoch1).TotalMilliseconds);


        }

        private static double calY(double y)
        {
            if (y == 0)
                return 0;

            double d = 0;
            int firstValue = 0;//最高位的值
            int len = 0;//d的长度
            string s;//存放d的字符串

            d = Math.Ceiling(Math.Abs(y));
            s = d.ToString();
            firstValue = int.Parse(s.Substring(0, 1));
            len = s.Length;
            //if (firstValue == 9)
            //    d = Math.Pow(10, len);
            //else
            //    d = (firstValue + 1) * Math.Pow(10, len - 1);
            Debug.WriteLine("0:  " + d);
            d = firstValue == 9 ? Math.Pow(10, len) : (firstValue + 1) * Math.Pow(10, len - 1);
            Debug.WriteLine("1:  " + d);
            if (d == double.NaN)
                Debug.WriteLine("");

            if (y < 0)
                d *= -1;


            return d;
        }

    }

    public class StaticTest
    {
        /// <summary>
        /// 定义委托
        /// </summary>
        /// <param name="user">用户</param>
        delegate void MakeStaticDelegate(string user);

        private static string lastUser = string.Empty;
        /// <summary>
        /// 这里是测试静态方法
        /// </summary>
        /// <param name="user">用户</param>
        private static void MakeStaticTest(string user)//static or not
        {
            for (int i = 0; i < 10; i++)
            {
                // 输出当前的变量
                System.Console.WriteLine(user + "：" + i.ToString() + "\t" + lastUser);
                //System.Threading.Thread.Sleep(new Random().Next(500,800));
                lastUser = user;
            }
        }

        /// <summary>
        /// 这里是模拟多用户同时点击并发
        /// 静态变量会有脏数据
        /// </summary>
        public void DoTest()
        {
            // 模拟3个用户的并发操作
            MakeStaticDelegate makeStaticDelegate1 = new MakeStaticDelegate(MakeStaticTest);
            makeStaticDelegate1.BeginInvoke("user1", null, null);
            MakeStaticDelegate makeStaticDelegate2 = new MakeStaticDelegate(MakeStaticTest);
            makeStaticDelegate2.BeginInvoke("user2", null, null);
            MakeStaticDelegate makeStaticDelegate3 = new MakeStaticDelegate(MakeStaticTest);
            makeStaticDelegate3.BeginInvoke("user3", null, null);
            //System.Console.ReadLine();
        }
    }
}
