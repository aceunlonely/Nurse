using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nurse.Common.DDD
{
    /// <summary>
    /// 配置节点
    /// </summary>
    [Serializable]
    public class MSMQConfigNode
    {
        /// <summary>
        /// 实例名
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Domain标记
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 种类
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 计数器名
        /// </summary>
        public string CounterName { get; set; }
    }
}
