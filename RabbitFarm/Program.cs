using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitFarm
{
    class Program
    {
        static void Main(string[] args)
        {
            Tag:
            Console.Write("请输入要测试的年数：");
            string input = Console.ReadLine();
            double dyear;
            if(double.TryParse(input,out dyear)){
                Farms fs = new Farms();
                int count = fs.GetNum(dyear);
                Console.WriteLine("\r\n{0}年以后兔子的对数为：{1}",dyear,count);
            }
            else{
                Console.WriteLine("\r\n您输入的有误\r\n");
                goto Tag;
            }

            Console.Read();
        }

        #region "兔子养殖场"
        public class Rabbit //兔子
        {
            private int rabbit_Type;    //第几代
            private double age; //年龄

            public Rabbit(int dai)
            {
                this.rabbit_Type = dai; //根据递归次数设置是第几代兔子，同代的兔子寿命相同，生育次数也相同
            }

            public int RabbitType
            {  //供外部查询是第几代兔子
                get { return this.rabbit_Type; }
            }

            public double Age
            {  //兔子年龄
                get { return this.age; }
                set { this.age = value; }
            }

        }

        public class Farms  //养殖场
        {
            private int era = 1;    //第1代开始
            private List<Rabbit> rlst;    //用于存放兔子的列表
            private double startTime;   //起始时间,默认0

            public Farms()
            {
                rlst = new List<Rabbit>();
                rlst.Add(new Rabbit(this.era)); //养殖场初始只有第一代兔子
            }

            public int GetNum(double nYear)
            {    //返回N年之后有多少对兔子
                if (nYear - this.startTime <= 0.0)
                {   //到达指定年限
                    return rlst.Count;  //返回有多少对兔子
                }

                Console.WriteLine("当前年份:{0}", this.startTime);

                List<Rabbit> nrs = rlst.FindAll(r => r.Age >= 1.5 && r.Age < 5.5);   //返回满足可生育的兔子

                if (nrs.Count > 0)
                { //如果有符合生育条件的兔子
                    this.era++; //代数自增             
                }
                for (int i = 0; i < nrs.Count; i++)
                {
                    Console.WriteLine("------谁生了：第{0}代，当前寿命{1}", nrs[i].RabbitType, nrs[i].Age);
                    rlst.Add(new Rabbit(this.era)); //添加出生的小兔子
                }
                nrs.Clear();    //本次清空

                int killNum = rlst.RemoveAll(r => r.Age >= 6.0);//等于6岁或超过6岁者,自然而然死!
                Console.WriteLine("========当前死亡数量:{0}", killNum);
                Console.WriteLine("========当前存活数量:{0}", rlst.Count);

                this.startTime += 0.5;  //年自增
                foreach (Rabbit rt in rlst)
                {
                    rt.Age += 0.5;    //年龄增加半年               
                }

                return GetNum(nYear);   //递归调用,返回结果值
            }
        }
        #endregion
    }
}
