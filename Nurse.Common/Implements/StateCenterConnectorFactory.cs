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
        public static IStateCenterConnector GetDefaultConnector()
        {
            switch (CommonConfig.ConnectorType)
            {
                case "web":
                    return new WebConnector();
                default:
                    CommonLog.InnerErrorLog.Error("配置存在问题，不支持的连接对象类型：" + CommonConfig.ConnectorType);
                    break;
            }
            return null;
        }
    }
}
