using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStateCenter.DDD
{
    /// <summary>
    /// 映射类
    /// </summary>
    [Serializable]
    public class Map
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}