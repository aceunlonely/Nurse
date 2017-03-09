using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Common.helper
{
    public class ProcessHelper
    {
        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="name">进程名</param>
        public static void CloseProcess(string name)
        {
            //关闭Slave进程
            Process[] ps = Process.GetProcessesByName(name);
            foreach (Process p in ps)
            {
                p.Kill();
                p.Close();
            }
            Thread.Sleep(100);
        }

        /// <summary>
        /// 开始进程
        /// </summary>
        /// <param name="path">路径</param>
        public static bool StartProcess(string path)
        {
            try
            {
                ProcessStartInfo procInfo = new ProcessStartInfo();
                Process proc = new Process();
                procInfo.FileName = path;
                procInfo.Arguments = "";
                //procInfo.WorkingDirectory = "";
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc = Process.Start(procInfo);
                Thread.Sleep(100);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 是否存在进程
        /// </summary>
        /// <param name="name">进程名</param>
        /// <returns></returns>
        public static bool ExistProcess(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes != null && processes.Length > 0) return true;
            return false;
        }
    }
}
