using Nurse.Common.Implements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Test.Service
{
    partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();

        }

        private bool IsRun = false;
        protected override void OnStart(string[] args)
        {
            IsRun = true;
            Thread thread = new Thread(new ThreadStart(Recycle));
            thread.IsBackground = true;
            thread.Start();
        }

        protected override void OnStop()
        {
            IsRun = false;
        }

        private void Recycle()
        {
            Test3();
        }

        public  void Test1()
        {
            for (int i = 1; ; i++)
            {
                if (!IsRun)
                {
                    break;
                }
                Console.WriteLine(i);

                Thread.Sleep(1000);
            }
        }

        public void Test2()
        {
            for (int i = 1; ; i++)
            {
                if (!IsRun)
                {
                    break;
                }
                BeatManager.Beat();
                Console.WriteLine(i);

                Thread.Sleep(1000);
            }
        }

        public void Test3()
        {
            BeatManager.Beat();
            for (int i = 1; ; i++)
            {
                Console.WriteLine(i);
                if (!IsRun)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
