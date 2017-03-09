using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Master.Service
{
    public class Common
    {
        private static bool _isRun = false;
        private static object _lockObj = new object();

        /// <summary>
        /// 是否运行
        /// </summary>
        public static bool IsRun
        {
            get
            {
                lock (_lockObj)
                {
                    return _isRun;
                }
            }

            set
            {
                lock (_lockObj) { _isRun = value; }
            }
        }
    }
}
