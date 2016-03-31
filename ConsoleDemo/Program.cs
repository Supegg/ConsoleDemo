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

            new StaticTest().DoTest();

            Console.Read();

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
