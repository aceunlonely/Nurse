using Nurse.Common.helper;
using Nurse.Slave.CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Slave
{
    class Program
    {

        private static ServiceManger _sm;
        public static ServiceManger SM
        {
            get
            {
                if (_sm == null)
                {
                    _sm = new ServiceManger();
                }
                return _sm;
            }
        }

        private static DLog _log;

        public static DLog Log {
            get
            {
                if (_log == null)
                {
                    _log = new DLog();
                    _log.Init("SlaveLog","SlaveLog/log");
                }
                return _log;
            }
        }
        static void Main(string[] args)
        {
            
            while(true)
            {
                //监控服务状态
                //Nurse.Master
                try
                {
                    if (SM.GetServiceValue(Config.MasterServiceName, "state").Equals(ServiceState.Stopped))
                    {
                        SM.StartService(Config.MasterServiceName);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("执行监控时出错(" + Config.MasterServiceName + ")：" + ex.ToString());
                }
                Thread.Sleep(Config.Internal);
            }
        }
    }
}
