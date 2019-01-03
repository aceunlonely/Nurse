using Nurse.Common.CM;
using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common
{
    public class CommonLog
    {
        private static object lock1 = new object();
        private static IDLog _innerErrorLog;



        public static IDLog InnerErrorLog
        {
            get
            {
                lock (lock1)
                {
                    if (_innerErrorLog == null)
                    {
                        _innerErrorLog =  new TinyLog();
                        _innerErrorLog.Init("InnerErrorLog", "InnerErrorLog/log");
                    }
                    return _innerErrorLog;
                }
            }
        }
    }
}
