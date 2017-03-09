using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.DDD
{
    /// <summary>
    /// 配置节点
    /// </summary>
    [Serializable]
    public class ConfigNode
    {
        /// <summary>
        /// ID 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 服务名 或者进程名
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用类型 1： 服务 2：可执行程序
        /// </summary>
        public int AppType { get; set; }

        /// <summary>
        /// 应用路径
        /// </summary>
        public string AppPath { get; set; }

        /// <summary>
        /// 守护类型 1：进程守护 2:心跳守护
        /// </summary>
        public int GuardType { get; set; }

        /// <summary>
        /// 守护间隔
        /// </summary>
        public int GuardInternal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 处理措施
        /// </summary>
        public List<HandleNode> Handles { get; set; }
    }
}
