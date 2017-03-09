using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Slave.CM
{
    public class Config
    {
        /// <summary>
        /// 通用间隔
        /// </summary>
        public static int Internal { get { return ConfigureHelper.GetConfigureIntValue("Internal", 10000); } }
    }
}
