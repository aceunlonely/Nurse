using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nurse.Common.DDD
{
    [Serializable]
    public class ConfigDomain
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
