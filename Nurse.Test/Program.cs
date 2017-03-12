using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch1 = new Stopwatch();

            watch1 = new Stopwatch();
            watch1.Start();

            new TestLog().Test();
            new TestCon().Test();
            new TestMq().Test();

            Console.WriteLine("测试方法运行时间：" + watch1.Elapsed);
            Console.Read();
        }
    }
}
