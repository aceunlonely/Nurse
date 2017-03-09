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
        /// <summary>
        /// ConnectorType
        /// </summary>
        public static string ConnectorType { get { return ConfigureHelper.GetConfigureValue("ConnectorType", "web"); } }


        public static string WebStateCenterUrl { get { return ConfigureHelper.GetConfigureValue("WebStateCenterUrl", "Error"); } }

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
    }
}
