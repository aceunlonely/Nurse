using Nurse.Common;
using Nurse.Common.DDD;
using Nurse.Common.helper;
using Nurse.Common.Implements;
using Nurse.Common.Interface;
using Nurse.Master.Service.CM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Nurse.Master.Service.Monitors
{
    /// <summary>
    /// windows 高级监控器
    /// </summary>
    public class WinMonitor
    {
        private static Thread thread;

        private static string _lastConfigTime;

        private static MSMQConfig _config;

        /// <summary>
        /// 执行监控
        /// </summary>
        public static void Run()
        {

            if (thread == null)
            {
                thread = new Thread(new ThreadStart(RecycleGuard));
                thread.Start();
            }
        }

        /// <summary>
        /// 核心方法
        /// </summary>
        private static void RecycleGuard()
        {
            //加载本地配置
            var mqConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config");
            try
            {
                if (File.Exists(mqConfig))
                    _config = XmlHelper.Xml2Entity(mqConfig, new MSMQConfig().GetType()) as MSMQConfig;
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("读取本地mq.config出错:" + ex.ToString() + "，继续逻辑");
            }
            int count = 0;
            while (Common.IsRun)
            {
                Thread.Sleep(Config.Internal);

                try
                {
                    //获取网络连接器
                    IStateCenterConnector connector = StateCenterConnectorFactory.GetConnector("web");

                    string lastConfigTime = connector.Promise("getLastConfigTime", "", "");
                    if (string.IsNullOrEmpty(lastConfigTime))
                    {
                        if (count++ > 10000)
                        {
                            //一直连不上
                            return;
                        }
                        //无法连接的web服务器时，等待重试
                        continue;
                    }
                    if (string.IsNullOrEmpty(_lastConfigTime) || lastConfigTime != _lastConfigTime)
                    {
                        _lastConfigTime = lastConfigTime;
                        //进行下载云端配置
                        string remoteConfig = connector.Promise("getMSMQConfig", "", "");
                        //空时，认为远端未配置,不进行读取执行
                        if (string.IsNullOrEmpty(remoteConfig) == false)
                        {
                            //存储到本地
                            File.WriteAllText(mqConfig, remoteConfig);

                            //重新加载配置
                            try
                            {
                                _config = XmlHelper.Xml2Entity(mqConfig, new MSMQConfig().GetType()) as MSMQConfig;
                            }
                            catch (Exception ex)
                            {
                                CommonLog.InnerErrorLog.Error("读取Remote mq.config出错:" + ex.ToString() + "，中断运行");
                                return;
                            }
                        }

                    }

                    //判断是否成功读取配置文件
                    if (_config == null)
                    {
                        //如果为空继续抓取远端配置
                        continue;
                    }
                    //执行监控
                    List<MonitorResult> mrs = WinMonitorHelper.RunMonitor(_config);
                    //推送数据到服务器
                    string msg = string.Empty;
                    foreach (MonitorResult mr in mrs)
                    {
                        msg += (mr.Domain ?? "") + "~" + (mr.CategoryName ?? "") + "~" + (mr.CounterName ?? "") + "~"
                            + (mr.Instance ?? "") + "~" + (mr.Result ?? "") + "~" + (mr.Remark ?? "") + "|";
                    }
                    connector.Promise("sendMonitorMsg", "key", msg.TrimEnd(new char[] { '|' }));

                }
                catch (Exception ex)
                {
                    CommonLog.InnerErrorLog.Error("mq监控出错:" + ex.ToString());
                }

            }

        }
    }
}
