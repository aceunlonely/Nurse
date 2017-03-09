using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.DDD
{
    /// <summary>
    ///  配置集合
    /// </summary>
    [Serializable]
    public class ConfigCollection
    {

        /// <summary>
        /// 配置
        /// </summary>
        public List<ConfigNode> Configs { get; set; }
    }
}
