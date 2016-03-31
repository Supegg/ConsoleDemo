using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadDemo
{
    class Program
    {
        //声明计数变量
        //（注意这里用的是long是64位的，所以在32位机子上一定要通过Interlocked来实现原子操作）
        static long _count = 0;

        static void Main(string[] args)
        {
            #region Interlocked
            //开启6个线程，3个执行Excution1，三个执行Excution2
            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(new ThreadStart(Excution1));
                Thread thread2 = new Thread(new ThreadStart(Excution2));
                thread.Start();
                Thread.Sleep(10);
                thread2.Start();
                Thread.Sleep(10);
            }
            //这里和同步无关，只是简单的对Interlocked方法进行示例
            Interlocked.Add(ref _count, 2);
            Console.WriteLine("为当前计数值加上一个数量级:{0}后，当前计数值为:{1}", 2, _count);
            Interlocked.Exchange(ref _count, 1);
            Console.WriteLine("将当前计数值改变后，当前计数值为:{0}", _count);
            #endregion




            Console.Read();
        }

        static void Excution1()
        {
            //进入共享区1的条件
            if (Interlocked.Read(ref _count) == 0)
            //if (_count == 0)
            {
                Console.WriteLine("Thread ID:{0} 进入了共享区1", Thread.CurrentThread.ManagedThreadId);
                //原子性增加计数值，让其他线程进入共享区2
                Interlocked.Increment(ref _count);
                Console.WriteLine("此时计数值Count为:{0}", Interlocked.Read(ref _count));
            }
        }

        static void Excution2()
        {
            //进入共享区2的条件
            if (Interlocked.Read(ref _count) == 1)
            //if (_count == 1)
            {
                Console.WriteLine("Thread ID:{0} 进入了共享区2", Thread.CurrentThread.ManagedThreadId);
                //原子性减少计数值，让其他线程进入共享区1
                Interlocked.Decrement(ref _count);
                Console.WriteLine("此时计数值Count为:{0}", Interlocked.Read(ref _count));
            }
        }
    }
}
