using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace DBTest
{
    class Program
    {
        static string conString = "Data Source=.;Initial Catalog=MyDemo;User ID=supegg;Password=sens0508;";
        static Stopwatch sw = new Stopwatch();
        static SqlCommand cm;

        static void Main(string[] args)
        {
            cm = new SqlCommand();
            cm.Connection = new SqlConnection(conString);
            cm.Parameters.Add("name", SqlDbType.NVarChar);
            cm.Parameters.Add("age", SqlDbType.Int);

            //Console.WriteLine("Insert time:" + testInsert() +" ms");
            for (var i = 5; i <= 5; i++)
            {
                Console.WriteLine("Procedure time:" + testProcedure("m" + i, i * 1000000) + " s");
            }

            Console.Read();

        }

        /// <summary>
        /// 第一个100w数据插入时间 331s
        /// 200w  672s
        /// 300w  1081s
        /// 400w  
        /// 500w  1857s
        /// 600w  
        /// 700w  
        /// 800w  
        /// 900w
        /// 1000w
        /// </summary>
        /// <returns></returns>
        static double testProcedure(string tname, int count)
        {
            //cm.Parameters.Add("tname", SqlDbType.NVarChar);
            //cm.Parameters["tname"].Value = tname;
            //cm.Parameters.Add("name", SqlDbType.NVarChar);
            //cm.Parameters.Add("age", SqlDbType.Int);
            cm.CommandType = CommandType.Text;
            //cm.CommandText = "Insert_m1";
            cm.CommandText = "insert into "+tname+" (name,age) values (@name,@age)";
            
            int i = 0;
            Random r = new Random();
            sw.Reset();
            sw.Start();
            cm.Connection.Open();
            while (i++ <count)
            {
                cm.Parameters["name"].Value = "su" + i;
                cm.Parameters["age"].Value = r.Next(1,100);
                cm.ExecuteNonQuery();

                if (i % 100000 == 0)
                    Console.WriteLine("no. "+i +",time: "+sw.Elapsed.TotalSeconds+ " s");
            }
            cm.Connection.Close();
            sw.Stop();
            return sw.Elapsed.TotalSeconds;
        }

        static double testInsert()
        {
            cm.Parameters.Clear();
            cm.CommandType = CommandType.Text;
            cm.CommandText = "insert into dbo.Table_List ([name],[description]) values('test','this is a test')";

            int i = 0;
            sw.Reset();
            sw.Start();
            while (i < 1000000)
            {
                i++;
                cm.Connection.Open();
                cm.ExecuteNonQuery();
                cm.Connection.Close();

                if (i % 10000 == 0)
                    Console.WriteLine("no. " + i + ",time: " + sw.Elapsed.TotalMilliseconds + " ms");
            }
            sw.Stop();
            return sw.Elapsed.TotalMilliseconds;
        }
    }
}
