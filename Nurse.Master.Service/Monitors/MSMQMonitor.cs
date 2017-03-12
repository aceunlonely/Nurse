using Nurse.Common;
using Nurse.Common.helper;
using Nurse.Common.Implements;
using Nurse.Common.Interface;
using Nurse.Master.Service.CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Nurse.Master.Service.Monitors
{
    public class MSMQMonitor
    {
        private static Thread thread;

        private static List<string> _msConfigList;
        private MSMQMonitor() { }

        public static void StartRun(List<string> list)
        {
            _msConfigList = list;
            if (thread == null)
            {
                thread = new Thread(new ThreadStart(RecycleGuard));
                thread.Start();
            }
        }


        private static void RecycleGuard()
        {
            while (Common.IsRun)
            {
                try
                {
                    //获取所有监控状态
                    List<MqCount> list = MSMQHelper.GetMqCount(_msConfigList);

                    if (list != null && list.Count > 0)
                    {
                        IStateCenterConnector connector = StateCenterConnectorFactory.GetAvailableConnector();
                        string msg = string.Empty;
                        foreach (MqCount mq in list)
                        {
                            msg += mq.Name + "@" + mq.Count + "@" + mq.Remark + "|";
                        }
                        connector.SendMsg("msmq", msg);
                    }
                }
                catch (Exception ex)
                {
                    CommonLog.InnerErrorLog.Error("mq监控出错:" + ex.ToString());
                }
                Thread.Sleep(Config.Internal);
            }

        }
    }
}
