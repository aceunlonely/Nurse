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
    /// 监控帮助类
    /// </summary>
    public class WinMonitorHelper
    {
        /// <summary>
        /// 监视器
        /// </summary>
        private static Dictionary<string, PerformanceCounterRetriever> _performanceMap = new Dictionary<string, PerformanceCounterRetriever>();

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

                        //本机ip不做处理
                        if (ComputerInfo.GetIPAddress() == strDomainInfos[0])
                            continue;

                        _performanceMap.Add(domain.Name, new PerformanceCounterRetriever(strDomainInfos[0], strDomainInfos[1], strDomainInfos[2], strDomainInfos[3]));
                    }
                    catch (Exception ex)
                    {
                        CommonLog.InnerErrorLog.Error("内部解析Domain信息，或者初始化出错:" + ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 获取性能计数器
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="categoryName"></param>
        /// <param name="counterName"></param>
        /// <param name="optionalInstanceName"></param>
        /// <returns></returns>
        private static PerformanceCounter GetPerformanceCounter(string domainName, string categoryName, string counterName, string optionalInstanceName = null)
        {
            //存在默认值
            if (string.IsNullOrEmpty(categoryName))
                categoryName = "MSMQ Queue";
            if (string.IsNullOrEmpty(counterName))
                counterName = "Messages in Queue";
            if (string.IsNullOrEmpty(domainName) || domainName == ComputerInfo.GetIPAddress())
            {
                return new PerformanceCounter(categoryName, counterName, optionalInstanceName);
            }
            if (_performanceMap.ContainsKey(domainName))
            {
                return _performanceMap[domainName].GetCounter(categoryName, counterName, optionalInstanceName);
            }
            return null;
        }

        /// <summary>
        /// 执行监控
        /// </summary>
        /// <param name="config">监控配置</param>
        /// <returns>结果列表</returns>
        public static List<MonitorResult> RunMonitor(MSMQConfig config)
        {
            if (config == null || config.Nodes == null) return null;
            //初始化所有domain
            DomainInit(config.Domains);
            List<MonitorResult> rs = new List<MonitorResult>();
            foreach (MSMQConfigNode node in config.Nodes)
            {
                MonitorResult mr = new MonitorResult()
                {
                    CategoryName = node.CategoryName,
                    CounterName = node.CounterName,
                    Domain = node.Domain,
                    Instance = node.Instance,
                    Remark = string.Empty
                };
                try
                {
                    var counter = GetPerformanceCounter(node.Domain, node.CategoryName, node.CounterName, node.Instance);
                    if (counter == null)
                    {
                        mr.Remark = "获取监控器出错，请检查是否正确配置Domain";
                        mr.Result = "";
                    }
                    else
                    {
                        mr.Result = counter.NextValue().ToString();

                    }
                }
                catch(Exception ex) 
                {
                    mr.Remark = "节点获取异常：" + (CommonConfig.IsDebug ? ex.ToString() : "");
                }
                rs.Add(mr);

            }
            return rs;
        }


    }

    /// <summary>
    /// 监控结果
    /// </summary>
    public class MonitorResult
    {
        /// <summary>
        /// 实例名
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Domain标记
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 种类
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 计数器名
        /// </summary>
        public string CounterName { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public String Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 重写tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (this.Domain ?? "") + (this.CategoryName ?? "") + (this.CounterName ?? "") + (this.Instance ?? "");
        }
    }
}
