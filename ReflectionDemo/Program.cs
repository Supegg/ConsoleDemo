using System;
using System.Reflection;
using System.Runtime.Remoting;

namespace ReflectionDemo
{

    #region "程序入口"
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("先载入一个程序集，然后分析它的结构:");
            //System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom("ReflectionDemo.exe");//从文件载入程序集
            //System.Reflection.Assembly ass = System.Reflection.Assembly.Load("ReflectionDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");//从程序集信息载入程序集
            Assembly ass = Assembly.GetExecutingAssembly();    //加载当前程序集
            Console.WriteLine("程序集的名称是:{0}", ass.FullName);
            Console.WriteLine(new string('-', 40));

            //列出包含的类
            Type[] tss = ass.GetTypes();
            foreach (Type ts in tss)
            {
                Console.WriteLine("类型名:{0}", ts.Name);
            }

            Console.WriteLine(new string('-', 40));
            //单独针对TestClass反射调用
            Type Oa = ass.GetType("ReflectionDemo.TestClass");
            Console.WriteLine("当前类型名:{0}", Oa.Name);

            MemberInfo[] minss = Oa.GetMembers(BindingFlags.Instance |
                                                  BindingFlags.Static |
                                                 BindingFlags.Public |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.DeclaredOnly);    //获取所有成员
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("成员列表:");
            foreach (MemberInfo mins in minss)
            {
                Console.WriteLine("{0}", mins);
            }
            Console.WriteLine(new string('-', 40));

            //实例化一个TestClass对象

            //1.使用公共的构造函数实例化，带一个参数
            //使用程序集Assembly.CreateInstance()进行实例化
            //第一个参数：代表了要创建的类型实例的字符串名称
            //第二个参数：说明是不是大小写无关(Ignore Case)
            //第三个参数：在这里指定Default，意思是不使用BingdingFlags的策略(你可以把它理解成null，但是BindingFlags是值类型，所以不可能为null，必须有一个默认值，而这个Default就是它的默认值)；
            //第四个参数：是Binder，它封装了CreateInstance绑定对象(Calculator)的规则，我们几乎永远都会传递null进去，实际上使用的是预定义的DefaultBinder；
            //第五个参数：是一个Object[]数组类型，它包含我们传递进去的参数，有参数的构造函数将会使用这些参数；
            //第六个参数：是一个CultureInfo类型，它包含了关于语言和文化的信息(简单点理解就是什么时候ToString("c")应该显示“￥”，什么时候应该显示“＄”)。
            //第七个参数：是一个object[]数组，描述特性

            //Ta就是我们实例化后的一个TestClass对象了。
            //这是调用的公共构造函数
            object Ta = ass.CreateInstance(Oa.FullName, true, BindingFlags.Default, null, new object[] { "麦克" }, null, null);

            //这是调用的private构造函数
            //方式用的是 Activator.CreateInstance()进行实例化，返回一个ObjectHandle类的对象
            //需要Unwrap()才能返回object对象
            //Activator.CreateInstance()的参数说明
            //第一个参数：当前程序集的全名称，字符串的形式
            //第二个参数：代表了要创建的类型实例的字符串名称
            //第三个参数：说明是不是大小写无关(Ignore Case)
            //第四个参数：BindingFlags
            //              Default，意思是不使用BingdingFlags的策略
            //              NonPublic指定是非公共的类型
            //第五个参数：是Binder，它封装了CreateInstance绑定对象(Calculator)的规则，我们几乎永远都会传递null进去，实际上使用的是预定义的DefaultBinder；
            //第六个参数：是一个Object[]数组类型，它包含我们传递进去的参数，有参数的构造函数将会使用这些参数；
            //其他参数：……略
            ObjectHandle handler = Activator.CreateInstance(null, Oa.FullName, true, BindingFlags.Default |
                                                            BindingFlags.Instance |
                                                               BindingFlags.NonPublic, null, null, null, null, null);
            //Tb是通过私有的构造函数创建的对象
            object Tb = handler.Unwrap();

            //调用其方法进行做些事情
            Console.WriteLine("调用其方法进行做些事情:");

            Oa.InvokeMember("Show", BindingFlags.InvokeMethod, null, Ta, null, null, null, null);
            //Pshow方法是private，依然无阻力调用
            Oa.InvokeMember("Pshow", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, Ta, null, null, null, null);

            Console.WriteLine();
            Oa.InvokeMember("Show", BindingFlags.InvokeMethod, null, Tb, null, null, null, null);
            Oa.InvokeMember("Pshow", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, Tb, null, null, null, null);

            Console.WriteLine();
            //通过属性后台生成的方法，查阅了一次属性
            string str = Oa.InvokeMember("get_ClassName", BindingFlags.InvokeMethod, null, Tb, null, null, null, null).ToString();
            Console.WriteLine("Tb属性名:{0}", str);

            Console.WriteLine();
            //通过属性后台生成的方法，设置了一次属性，属性的set访问器是private
            Oa.InvokeMember("set_ClassName", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, Tb, new object[] { "撒旦" }, null, null, null);

            Console.WriteLine();
            //通过属性后台生成的方法，再次查阅了更改后的属性
            str = Oa.InvokeMember("get_ClassName", BindingFlags.InvokeMethod, null, Tb, null, null, null, null).ToString();
            Console.WriteLine("Tb属性名:{0}", str);

            Console.WriteLine(new string('-', 40));
            Console.WriteLine("属性：");
            str = Oa.InvokeMember("ClassName", BindingFlags.GetProperty, null, Tb, null, null, null, null).ToString();
            Console.WriteLine("{1}获取到的属性是:{0}", str, "Tb");

            //这个设置属性很有意思
            //整体是public，而set却是private，所以BindingFlags就需要NonPublic和Public，缺一个都不行
            Console.WriteLine();
            Oa.InvokeMember("ClassName", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, Tb, new object[] { "吸血鬼" }, null, null, null);

            Console.WriteLine();
            str = Oa.InvokeMember("ClassName", BindingFlags.GetProperty, null, Tb, null, null, null, null).ToString();
            Console.WriteLine("{1}获取到的属性是:{0}", str, "Tb");

            Console.WriteLine(new string('-', 40));
            Console.WriteLine("直接访问或设置私有字段：");
            string field = Oa.InvokeMember("className", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic, null, Tb, null, null, null, null).ToString();
            Console.WriteLine("Tb's private string className:{0}", field);

            //直接设置类中的私有字段
            Oa.InvokeMember("className", BindingFlags.SetField | BindingFlags.Instance | BindingFlags.NonPublic, null, Tb, new object[] { "血之修罗" }, null, null, null);

            Console.WriteLine("\n重新设置后：");
            field = Oa.InvokeMember("className", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic, null, Tb, null, null, null, null).ToString();
            Console.WriteLine("Tb's private string className:{0}", field);

            Console.WriteLine();
            Console.WriteLine("反射让你的代码无所遁形，如果你的代码没有做一些防护措施的话。");

            Console.Read();
        }
    }
    #endregion

    #region "待反射调用的类"
    public class TestClass
    {
        private string className;

        private TestClass()
        {
            this.className = "深渊恶魔";
        }

        public TestClass(string n)
        {
            this.className = n;
        }

        public void Show()
        {
            Console.WriteLine("className is:{0}", this.className);
        }

        private void Pshow()
        {
            Console.WriteLine("万恶的家伙，您不应该调用此方法。它的名字是:{0}", this.className);
        }

        public string ClassName
        {
            get { return this.className; }
            private set { Console.WriteLine("正在调用私有的属性设置器，破坏规则的家伙真讨厌！"); this.className = value; }
        }

    }
    #endregion
}