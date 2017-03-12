using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Nurse.Common.helper
{
    public class MSMQHelper
    {

        /// <summary>
        /// 获取mq深度
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<MqCount> GetMqCount(List<string> list)
        {
            if (list == null) return null;
            List<MqCount> arrResult = new List<MqCount>();
            PerformanceCounterCategory countCategory = new PerformanceCounterCategory("MSMQ Queue");
            //所有消息队列数量
            PerformanceCounter allCount = new PerformanceCounter("MSMQ Queue", "Messages in Queue");
            foreach (string instanceName in list)
            {
                string remark = "";
                string count = "";
                try
                {
                    allCount.InstanceName = instanceName;//需要给实例名赋值
                    count = allCount.NextValue().ToString();
                }
                catch (Exception ex)
                {
                    remark = "节点获取异常";
                }
                arrResult.Add(new MqCount()
                {
                    Name = instanceName,
                    Count = count,
                    Remark = remark
                });
            }

            return arrResult;
        }
    }

    public class MqCount
    {
        public string Name { get; set; }
        public string Count { get; set; }

        public string Remark { get; set; }
    }
}
