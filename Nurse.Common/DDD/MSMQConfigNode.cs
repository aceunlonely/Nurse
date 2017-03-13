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
        public string Instance { get; set; }

        public string Domain { get; set; }
    }
}
