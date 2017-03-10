using Nurse.Common.CM;
using Nurse.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.Implements
{
    public class StateCenterConnectorFactory
    {

        /// <summary>
        /// 获取系统默认的连接器
        /// </summary>
        /// <returns></returns>
        public static IStateCenterConnector GetConnector(string connectorType = "")
        {

            switch (connectorType)
            {
                case "web":
                    return new WebConnector();
                case "disk":
                    return new DiskConnector();
                default:
                    CommonLog.InnerErrorLog.Error("配置存在问题，不支持的连接对象类型：" + connectorType);
                    break;
            }
            return null;
        }

        /// <summary>
        /// 寻找可用的状态中心
        /// </summary>
        /// <returns></returns>
        public static IStateCenterConnector GetAvailableConnector()
        {
            IStateCenterConnector connector = GetConnector(CommonConfig.ConnectorType);
            if (connector == null || connector.IsCenterAlived() == false)
            {
                connector = GetConnector(CommonConfig.BackupConnectorType);
            }
            return connector;
        }
    }
}
