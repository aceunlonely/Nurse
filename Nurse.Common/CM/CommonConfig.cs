using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.CM
{
    public class CommonConfig
    {
        public static bool IsDebug { get { return ConfigureHelper.GetConfigureBoolValue("IsDebug", false); } }
        /// <summary>
        /// ConnectorType
        /// </summary>
        public static string ConnectorType { get { return ConfigureHelper.GetConfigureValue("ConnectorType", "web"); } }

        /// <summary>
        /// 只有配置ConnectorType或者BackupConnectorType 有 web的时候需要配置这个节点
        /// </summary>
        public static string WebStateCenterUrl { get { return ConfigureHelper.GetConfigureValue("WebStateCenterUrl", "Error"); } }

        /// <summary>
        /// 备用连接器类型
        /// </summary>
        public static string BackupConnectorType { get { return ConfigureHelper.GetConfigureValue("BackupConnectorType", "disk"); } }

        /// <summary>
        /// 只有配置ConnectorType或者BackupConnectorType 有 disk的时候需要配置这个节点，当然也可以采用默认值
        /// </summary>
        public static string DiskStateCenterPath { get { return ConfigureHelper.GetConfigureValue("DiskStateCenterPath", "D:/Nurse/DiskStateCenter"); } }

        /// <summary>
        /// 心跳间隔， beatManager同步心跳时间到状态中心的时间间隔
        /// </summary>
        public static int BeatInternal
        {
            get
            {
                var beatInternal = ConfigureHelper.GetConfigureIntValue("BeatInternal", 10000);
                return beatInternal > 60 * 1000 ? 60 * 1000 : beatInternal;
            }
        }

        /// <summary>
        /// 心跳配置
        /// </summary>
        public static string BeatAppName { get { return ConfigureHelper.GetConfigureValue("BeatAppName", ""); } }

        /// <summary>
        /// 是否加密
        /// </summary>
        public static bool IsEncrypt { get { return ConfigureHelper.GetConfigureBoolValue("IsEncrypt", false); } }

        /// <summary>
        /// 加密密码
        /// </summary>
        public static string EncryptKey { get { return ConfigureHelper.GetConfigureValue("EncryptKey", "AS4tv6jcIZ4F7MNMZpvH4Q=="); } }

        
    }
}
