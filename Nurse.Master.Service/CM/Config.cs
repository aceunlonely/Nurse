using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Master.Service.CM
{
    public class Config
    {
        /// <summary>
        /// 是否一直运行， 如果开启，就相互开启
        /// </summary>
        public static bool IsAlwaysRun { get { return ConfigureHelper.GetConfigureBoolValue("IsAlwaysRun", true); } }

        /// <summary>
        /// 通用间隔
        /// </summary>
        public static int Internal { get { return ConfigureHelper.GetConfigureIntValue("Internal", 1000); } }

        /// <summary>
        /// 检查Slave的时间间隔
        /// </summary>
        public static int Internal_Check_Slave { get { return ConfigureHelper.GetConfigureIntValue("Internal_Check_Slave", Internal); } }

        /// <summary>
        /// 服务重启重试次数 默认1200次（10分钟）
        /// </summary>
        public static int ServiceRebootReTryCount { get { return ConfigureHelper.GetConfigureIntValue("ServiceRebootReTryCount", 1200); } }
    }
}
