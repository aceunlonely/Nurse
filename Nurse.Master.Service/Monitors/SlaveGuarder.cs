using Nurse.Common;
using Nurse.Common.helper;
using Nurse.Master.Service.CM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Master.Service.Monitors
{
    public class SlaveGuarder
    {
        private static Thread thread;
        private SlaveGuarder() { }

        public static void StartRun()
        {
            if (thread == null)
            {
                thread = new Thread(new ThreadStart(RecycleGuard));
                thread.Start();
            }
        }


        private static void RecycleGuard()
        {
            string strSlaveName = "Nurse.Slave";
            while (Common.IsRun)
            {
                try
                {
                    if (ProcessHelper.ExistProcess(strSlaveName) == false)
                    {
                        int count = 0;
                        string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Nurse.Slave.exe");
                        while (ProcessHelper.StartProcess(strPath) == false)
                        {
                            if (count++ > 4)
                            {
                                CommonLog.InnerErrorLog.Error("多次启动Nurse.Slave失败，请查看安装包里是否有Nurse.Slave.exe文件");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonLog.InnerErrorLog.Error("Master守护slave的主程序运行出现异常！：" + ex.ToString());
                }
                Thread.Sleep(Config.Internal_Check_Slave);
            }

            // 只有当 不执行时才会主动关闭slave 进程
            if (Config.IsAlwaysRun == false)
            {
                //关闭Slave进程
                ProcessHelper.CloseProcess(strSlaveName);
            }
        }
    }
}
