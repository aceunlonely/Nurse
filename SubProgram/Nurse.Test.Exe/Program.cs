using Nurse.Common.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Test.Exe
{
    class Program
    {
        static void Main(string[] args)
        {
            //test
            Test2();
        }

        public static void Test1()
        {
            for (int i = 1; ; i++)
            {
                Console.WriteLine(i);

                Thread.Sleep(1000);
            }
        }

        public static void Test2()
        {
            for (int i = 1; ; i++)
            {
                BeatManager.Beat();
                Console.WriteLine(i);

                Thread.Sleep(1000);
            }
        }

        public static void Test3()
        {
            BeatManager.Beat();
            for (int i = 1; ; i++)
            {
                Console.WriteLine(i);

                Thread.Sleep(1000);
            }
        }
    }
}
