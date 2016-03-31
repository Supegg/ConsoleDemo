using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Inherit
{
    class A
    {
        /// <summary>
        /// this is A
        /// </summary>
        public void Print()
        {
            Console.WriteLine("This is A.");
        }
    }

    class B:A
    {
        /// <summary>
        /// this is b
        /// </summary>
        public void Print()
        {
            Console.WriteLine("This is B.");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            A b = new B();
            
            a.Print();
            b.Print();

            Console.Read();
        }
    }
}
