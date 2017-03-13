﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nurse.Common.DDD
{
    [Serializable]
    public class MSMQConfig
    {
        public List<ConfigDomain> Domains { get; set; }
        public List<MSMQConfigNode> Nodes { get; set; }
    }
}
