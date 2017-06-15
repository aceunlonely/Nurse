using Dcjet.Framework.Helpers;
using Nurse.Common.CM;
using Nurse.Common.DDD;
using PerformanceReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Nurse.Common.helper
{
    /// <summary>
    /// MSMQ帮助类
    /// </summary>
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
                    remark = "节点获取异常" + (CommonConfig.IsDebug ? ex.ToString() : "");
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


        /// <summary>
        /// 获取mq深度
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<MqCount> GetMqCount(MSMQConfig config)
        {
            if (config.Nodes == null) return null;

            List<string> arrList = new List<string>();
            List<MSMQConfigNode> arrDomanList = new List<MSMQConfigNode>();
            foreach (MSMQConfigNode node in config.Nodes)
            {
                if (string.IsNullOrEmpty(node.Domain))
                {
                    arrList.Add(node.Instance);
                }
                else
                {
                    arrDomanList.Add(node);
                }
            }
            List<MqCount> arrMqCount = new List<MqCount>();
            if (arrList.Count > 0)
            {
                arrMqCount = GetMqCount(arrList);
            }
            DomainInit(config.Domains);
            foreach (MSMQConfigNode node in arrDomanList)
            {
                if (_performanceMap.ContainsKey(node.Domain))
                {
                    string remark = "";
                    string count = "";
                    try
                    {
                        //pc.Get("MSMQ Queue", "Messages in Queue", @"highvertest\private$\tx1")
                        count = _performanceMap[node.Domain].Get("MSMQ Queue", "Messages in Queue", node.Instance).ToString();
                    }
                    catch (Exception ex)
                    {
                        remark = "节点获取异常" + (CommonConfig.IsDebug ? ex.ToString() : "");
                    }
                    arrMqCount.Add(new MqCount()
                    {
                        Name = node.Instance,
                        Count = count,
                        Remark = remark
                    });
                }
                else
                {
                    throw new Exception("未配置正确的Domain: " + node.Domain);
                }
            }
            return arrMqCount;
        }

        private static void DomainInit(List<ConfigDomain> domains)
        {
            if (domains == null) return;
            foreach (ConfigDomain domain in domains)
            {
                if (_performanceMap.ContainsKey(domain.Name) == false)
                {
                    try
                    {
                        //解密value
                        string value = EncryptHelper.DecryptDES(domain.Value);
                        //"192.168.10.228|WORKGROUP|administrator|dcjet@888";
                        string[] strDomainInfos = value.Split(new char[] { '|' });

                        _performanceMap.Add(domain.Name, new PerformanceCounterRetriever(strDomainInfos[0], strDomainInfos[1], strDomainInfos[2], strDomainInfos[3]));
                    }
                    catch (Exception ex)
                    {
                        CommonLog.InnerErrorLog.Error("内部解析Domain信息，或者初始化出错:" + ex.ToString());
                    }
                }
            }
        }

        private static Dictionary<string, PerformanceCounterRetriever> _performanceMap = new Dictionary<string, PerformanceCounterRetriever>();
    }

    public class MqCount
    {
        public string Name { get; set; }
        public string Count { get; set; }

        public string Remark { get; set; }
    }
}
