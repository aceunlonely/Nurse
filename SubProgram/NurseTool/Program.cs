using Nurse.Common.DDD;
using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NurseTool
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "mssend":
                    if (args.Length > 2)
                    {
                        TestSend(args[1], args[2]);
                    }
                    else
                    {
                        TestSend(args[1]);
                    }
                    break;
                case "mscount":
                    TestGetCount(args[1]);
                    break;
                case "msallcount":
                    TestAllCount();
                    break;
                case "msconfig":

                    var mqConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config");
                    if (File.Exists(mqConfig) == false)
                    {
                        Console.WriteLine("找不到" + mqConfig);
                    }
                    else
                    {
                        MSMQConfig msmqConfig = null;
                        msmqConfig = XmlHelper.Xml2Entity(mqConfig, new MSMQConfig().GetType()) as MSMQConfig;
                        List<MqCount> list = MSMQHelper.GetMqCount(msmqConfig);
                        foreach (MqCount mc in list)
                        {
                            Console.WriteLine(string.Format(" {0} | {1} | {2}",mc.Name,mc.Count,mc.Remark));
                        }
                    }
                    break;
                default:
                    Console.WriteLine("nothing todo ");
                    break;

            }
            Console.Read();

        }

        public static void TestSend(string mq, string strMsg = "test")
        {
            string strMq = mq;//@".\Private$\lxy2";
            MessageQueue mqQue = new MessageQueue(strMq);
            mqQue.MessageReadPropertyFilter.SetAll();

            System.Messaging.Message msg = new System.Messaging.Message();
            //消息主体
            msg.Body = strMsg + "|" + DateTime.Now.ToString();
            //用描述设置ID
            msg.Label = "id123";
            //将消息加入到发送队列
            msg.ResponseQueue = mqQue;
            msg.AttachSenderId = true;

            msg.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(string) });

            try
            {
                //发送
                mqQue.Send(msg);
                Console.WriteLine("发送Msg成功:" + msg.Body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public static void TestGetCount(string instance)
        {
            try
            {
                PerformanceCounterCategory category = new PerformanceCounterCategory("MSMQ Queue");

                PerformanceCounter counter = new PerformanceCounter("MSMQ Queue", "Messages in Queue", instance);
                counter.InstanceName = instance;
                Console.WriteLine(string.Format("{0} 数量：{1}", counter.InstanceName, counter.NextValue().ToString()));

            }
            catch (Exception ex)
            {
                Console.WriteLine("错误:" + ex.ToString());
            }

        }

        public static void TestAllCount()
        {
            PerformanceCounterCategory countCategory = new PerformanceCounterCategory("MSMQ Queue");

            //所有消息队列数量
            PerformanceCounter allCount = new PerformanceCounter("MSMQ Queue", "Messages in Queue");
            foreach (string instanceName in countCategory.GetInstanceNames())
            {
                allCount.InstanceName = instanceName;//需要给实例名赋值
                Console.WriteLine(string.Format("{0} 数量：{1}", allCount.InstanceName, allCount.NextValue().ToString()));
            }
        }
    }
}
