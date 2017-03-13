using Dcjet.Framework.Helpers;
using Nurse.Common.DDD;
using Nurse.Common.helper;
using PerformanceReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;

namespace Nurse.Test
{
    class TestMq
    {
        public void Test()
        {
            //TestSend();

            //TestGetCount();
            //TestAllCount();
            //TestConfig();
            //TestGet();
            //TestConfig();
            TestDe();
        }

        public void TestDe()
        {
            Console.WriteLine(EncryptHelper.DecryptDES("ESvKEYyK/iZYFW2Zj16BRvdWGjElI+j75K5CLvjfMKgeIl0pvMRcvXWKU/roIuQNV37DiFNdhErwk7YWtXuL7AIn8z+4V6NFHLTLkwn8XsatMhL5OSsvzwAupJuzhbuJEHOINdRDopQ="));
        }

        public void TestGet()
        {
            PerformanceCounterRetriever pc = new PerformanceCounterRetriever("192.168.10.228", "WORKGROUP", "administrator", "dcjet@888");

            Console.WriteLine(pc.Get("MSMQ Queue", "Messages in Queue", @"highvertest\private$\tx1"));

        }

        public void TestConfig()
        {
            MSMQConfig mc = new MSMQConfig();
            string msg = "192.168.10.228|WORKGROUP|administrator|dcjet@888";
            string strMsg = "ESvKEYyK/iZYFW2Zj16BRvdWGjElI+j75K5CLvjfMKgeIl0pvMRcvXWKU/roIuQNV37DiFNdhErwk7YWtXuL7AIn8z+4V6NFHLTLkwn8XsatMhL5OSsvzwAupJuzhbuJEHOINdRDopQ=";
            mc.Domains = new List<ConfigDomain>();
            mc.Domains.Add(new ConfigDomain() { Name = "228", Value = strMsg });

            mc.Nodes = new List<MSMQConfigNode>();
            mc.Nodes.Add(new MSMQConfigNode() { Instance = @"xyliu\private$\lxy", Domain = "" });
            mc.Nodes.Add(new MSMQConfigNode() { Instance=@"highvertest\private$\tx1",Domain="228" });
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config.demo");
            XmlHelper.Enity2Xml(strPath, mc);
        }

        public void TestSend()
        {
            string strMq = @".\Private$\lxy2";
            MessageQueue mqQue = new MessageQueue(strMq);
            mqQue.MessageReadPropertyFilter.SetAll();

            System.Messaging.Message msg = new System.Messaging.Message();
            //消息主体
            msg.Body = "test " + DateTime.Now.ToString();
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
                Console.WriteLine("发送成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void TestGetCount()
        {
            //192.168.12.94/private$/xhg
            PerformanceCounterCategory category = new PerformanceCounterCategory("MSMQ Queue");
            string str = "lxy";
            while (true)
            {
                PerformanceCounter counter = new PerformanceCounter("MSMQ Queue", "Messages in Queue", Environment.MachineName + @"\private$\" + str);
                counter.InstanceName = Environment.MachineName + @"\private$\" + str;
                Console.WriteLine(string.Format("{0} 数量：{1}", counter.InstanceName, counter.NextValue().ToString()));
                Thread.Sleep(1000);
            }

        }

        public void TestAllCount()
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
