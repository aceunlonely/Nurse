using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nurse.Common.DDD
{
    [Serializable]
    public class MSMQConfig
    {
        public List<string> Nodes { get; set; }
    }
}
