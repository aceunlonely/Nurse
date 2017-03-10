using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStateCenter.DDD
{
    /// <summary>
    /// 映射集合
    /// </summary>
    [Serializable]
    public class MapCollection
    {
        public List<Map> Nodes { get; set; }
    }
}