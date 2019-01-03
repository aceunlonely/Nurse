
using Nurse.Common.DDD;
using Nurse.Common.helper;
using Nurse.Common.Helpers;
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
            //TestDe();
            //TestMSMQService();
            //TestRemoteMsMQService();
            //TestConfigRead();
            //TestGetIP();
            
        }

        public void TestGetIP()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(ComputerInfo.GetIPAddress());
                Thread.Sleep(500);
            }
        }


        public void TestRemoteMsMQService() {
            //PerformanceCounterRetriever pc = new PerformanceCounterRetriever("192.168.12.11", "WORKGROUP", "Administrator", "Lxy@12345");


            //
            PerformanceCounterRetriever pc = new PerformanceCounterRetriever("192.168.10.181", "WORKGROUP", "yanfa", "Dcjet@181");
            PerformanceCounter pfc1 = pc.GetCounter("MSMQ Service", "Outgoing Messages/sec");
            PerformanceCounter pfc2 = pc.GetCounter("MSMQ Service", "Incoming Messages/sec");

            while (true)
            {
   
                Console.WriteLine("Outgoing Messages/sec" + pfc1.NextValue().ToString());
                Console.WriteLine("Incoming Messages/sec" + pfc2.NextValue().ToString());
                Console.WriteLine("=========================================================");
                Thread.Sleep(500);
            }

            
        }

        public void TestMSMQService() {

            PerformanceCounterCategory countCategory = new PerformanceCounterCategory("MSMQ Service");
            
            //所有消息队列数量
            PerformanceCounter oms = new PerformanceCounter("MSMQ Service", "Outgoing Messages/sec");
            PerformanceCounter ims = new PerformanceCounter("MSMQ Service", "Incoming Messages/sec");
            //foreach (string instanceName in countCategory.GetInstanceNames())
            //{
            //    allCount.InstanceName = instanceName;//需要给实例名赋值
            //    Console.WriteLine(string.Format("{0} 数量：{1}", allCount.InstanceName, allCount.NextValue().ToString()));
            //}
            while (true)
            {
                Console.WriteLine("Outgoing Messages/sec" + oms.NextValue().ToString());
                Console.WriteLine("Incoming Messages/sec" + ims.NextValue().ToString());
                Console.WriteLine("=========================================================");
                Thread.Sleep(500);
            }

        }

        public void TestDe()
        {
            Console.WriteLine(EncryptHelper.DecryptDES("ESvKEYyK/iZYFW2Zj16BRvdWGjElI+j75K5CLvjfMKgeIl0pvMRcvXWKU/roIuQNV37DiFNdhErwk7YWtXuL7AIn8z+4V6NFHLTLkwn8XsatMhL5OSsvzwAupJuzhbuJEHOINdRDopQ="));
        }

        public void TestGet()
        {
            //PerformanceCounterRetriever pc = new PerformanceCounterRetriever("192.168.10.228", "WORKGROUP", "administrator", "dcjet@888");
            //PerformanceCounterRetriever pc = new PerformanceCounterRetriever("192.168.12.11", "WORKGROUP", "Administrator", "Lxy@12345");
            //
            PerformanceCounterRetriever pc = new PerformanceCounterRetriever("192.168.10.181", "WORKGROUP", "yanfa", "Dcjet@181");

            Console.WriteLine(pc.Get("MSMQ Queue", "Messages in Queue", @"apollo-bw\private$\lxy"));



        }

        public void TestConfig()
        {
            MSMQConfig mc = new MSMQConfig();
            string msg = "192.168.10.181|WORKGROUP|yanfa|xxxx@181";
            string strMsg = EncryptHelper.EncryptDES(msg);// "ESvKEYyK/iZYFW2Zj16BRvdWGjElI+j75K5CLvjfMKgeIl0pvMRcvXWKU/roIuQNV37DiFNdhErwk7YWtXuL7AIn8z+4V6NFHLTLkwn8XsatMhL5OSsvzwAupJuzhbuJEHOINdRDopQ=";
            mc.Domains = new List<ConfigDomain>();
            mc.Domains.Add(new ConfigDomain() { Name = "181", Value = strMsg });

            mc.Nodes = new List<MSMQConfigNode>();
            mc.Nodes.Add(new MSMQConfigNode() { Instance = @"private$\lxy", Domain = "181", CategoryName = "MSMQ Queue", CounterName = "Messages in Queue" });
            mc.Nodes.Add(new MSMQConfigNode() { Instance = "", Domain = "181", CategoryName = "MSMQ Service", CounterName = "Outgoing Messages/sec" });
            mc.Nodes.Add(new MSMQConfigNode() { Instance = "", Domain = "181", CategoryName = "MSMQ Service", CounterName = "Incoming Messages/sec" });
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config.demo");
            XmlHelper.Enity2Xml(strPath, mc);
        }

        public void TestConfigRead()
        {
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config.demo");
            MSMQConfig config = XmlHelper.Xml2Entity(strPath, new MSMQConfig().GetType()) as MSMQConfig;
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
