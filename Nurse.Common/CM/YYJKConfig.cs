using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nurse.Common.CM
{
    /// <summary>
    /// yyjk配置
    /// </summary>
    public class YYJKConfig
    {
        /// <summary>
        /// 接口提供方
        /// </summary>
        public static string YYJK_WebapiUrl { get { return ConfigureHelper.GetConfigureValue("YYJK_WebapiUrl", ""); } }

        /// <summary>
        /// 接口提供方
        /// </summary>
        public static string YYJK_Provider { get { return ConfigureHelper.GetConfigureValue("YYJK_Provider", "捷通科技"); } }

        /// <summary>
        /// yyjk中配置的系统code
        /// </summary>
        public static string YYJK_SystemCode { get { return ConfigureHelper.GetConfigureValue("YYJK_SystemCode", ""); } }

        /// <summary>
        /// yyjk中配置的机器标识
        /// </summary>
        public static string YYJK_MachineCode { get { return ConfigureHelper.GetConfigureValue("YYJK_MachineCode", ""); } }

        /// <summary>
        /// yyjk中配置的心跳监控项名称 ，默认为beat
        /// </summary>
        public static string YYJK_CollectItemKey { get { return ConfigureHelper.GetConfigureValue("YYJK_CollectItemKey", "beat"); } }

        /// <summary>
        /// yyjk中配置的主机IP
        /// </summary>
        public static string YYJK_HostIp { get { return ConfigureHelper.GetConfigureValue("YYJK_HostIp", ComputerInfo.GetIPAddress()); } }
    }
}
