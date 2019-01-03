using Nurse.Common.CM;
using Nurse.Common.helper;
using Nurse.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Nurse.Common.Implements
{
    /// <summary>
    /// 心跳管理器
    /// </summary>
    public class BeatManager
    {
        private static object lockObj = new object();
        private static object lockObj2 = new object();
        private static bool recycleRunState = false;
        private static DateTime? lastBeatTime = null;
        private static string AppName { get { return CommonConfig.BeatAppName; } }

        private static DateTime? LastBeatTime
        {
            get
            {
                lock (lockObj2)
                {
                    return lastBeatTime;
                }
            }
            set
            {
                lock (lockObj2)
                {
                    lastBeatTime = value;
                }
            }
        }

        private static bool RecycleRunState
        {
            get
            {
                lock (lockObj)
                {
                    return recycleRunState;
                }
            }
            set
            {
                lock (lockObj)
                {
                    recycleRunState = value;
                }
            }
        }


        /// <summary>
        /// 心跳
        /// </summary>
        public static void Beat()
        {
            LastBeatTime = DateTime.Now;
            if (RecycleRunState == false)
            {
                RecycleRunState = true;
                Thread thread = new Thread(new ThreadStart(Recycle));
                //设置为后台线程
                thread.IsBackground = true;
                thread.Start();
            }
        }

        /// <summary>
        /// 内部循环
        /// </summary>
        private static void Recycle()
        {
            // 创建自己的日志
            IDLog log = new TinyLog();
            log.Init("beat", "beat/log");



            if (string.IsNullOrEmpty(AppName))
            {
                log.Error("节点BeatAppName 未配置，请配置");
                return;
            }
            string beatName = ComputerInfo.GetMacAddress() + "." + AppName;
            while (RecycleRunState)
            {
                log.Info("开始一次心跳");
                IStateCenterConnector connector = StateCenterConnectorFactory.GetAvailableConnector();
                if (connector == null)
                {
                    log.Error("没有找到可用的状态中心连接器");
                }
                else
                {
                    try
                    {
                        //不管有没有值，都发送到远端
                        log.Info("发送到远端" + beatName);
                        if (connector.Beat(beatName, LastBeatTime))
                        {
                            log.Info("发送成功");
                        }
                        else
                        {
                            log.Error("发送失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("心跳出错：" + ex.ToString());
                    }
                }
                Thread.Sleep(CommonConfig.BeatInternal);
            }

        }
    }
}
