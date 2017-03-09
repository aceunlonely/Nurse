using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.DDD
{
    /// <summary>
    /// 处理节点
    /// </summary>
    [Serializable]
    public class HandleNode
    {
        /// <summary>
        /// 条件
        /// </summary>
        public int Condition { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 计划 
        /// </summary>
        public int Plan { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
}
