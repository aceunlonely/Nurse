using Nurse.Common.CM;
using Nurse.Common.DDD;
using Nurse.Common.helper;
using Nurse.Master.Service.CM;
using Nurse.Master.Service.Monitors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Master.Service
{
    partial class MainService : ServiceBase
    {
        public MainService()
        {
            log = new TinyLog();
            log.Init("main", "main/log");
            InitializeComponent();
        }

        public IDLog log = null;

        protected override void OnStart(string[] args)
        {
            Common.IsRun = true;
            DispatchTask();
            log.Info("NurseMaster服务开启");
        }

        protected override void OnStop()
        {
            Common.IsRun = false;

            if (Config.IsAlwaysRun == false)
            {
                //关闭slave进程
                if (ProcessHelper.ExistProcess("Nurse.Slave"))
                {
                    ProcessHelper.CloseProcess("Nurse.Slave");
                }
            }
            log.Info("NurseMaster服务关闭");
        }

        /// <summary>
        /// 分发任务
        /// </summary>
        private void DispatchTask()
        {
            //仆人监控开始
            SlaveGuarder.StartRun();
            // 一般监控
            #region 服务和exe监控
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nurse.config");

            if (File.Exists(path) == false)
            {
                log.Info("程序未能找到配置文件: " + path);
            }
            else
            {
                ConfigCollection cc = null;
                try
                {
                    cc = XmlHelper.Xml2Entity(path, new ConfigCollection().GetType()) as ConfigCollection;
                }
                catch (Exception ex)
                {
                    log.Error("解析配置文件出错： " + ex.ToString());
                    return;
                }
                //执行监控逻辑
                foreach (ConfigNode node in cc.Configs)
                {
                    //服务时
                    if (node.AppType == (int)Enums.EnumAppType.服务)
                    {
                        log.Info(string.Format("开启服务监控： ID[{0}] 名称[{1}]", node.ID, node.AppName));
                        new CommonGuarder(node).Run();
                    }
                    else if (node.AppType == (int)Enums.EnumAppType.可执行程序)
                    {
                        log.Info(string.Format("开启运行程序监控： ID[{0}] 名称[{1}] 程序路径[{2}]", node.ID, node.AppName, node.AppPath));
                        new CommonGuarder(node).Run();
                    }
                    else
                    {
                        log.Error(string.Format("错误的配置节点： ID[{0}] 名称[{1}]", node.ID, node.AppName));
                    }
                }
            }
            #endregion

            //监控msmq
            var mqConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config");
            //存在监控配置，就进行监控，否则不进行监控
            if (File.Exists(mqConfig) == false)
            {
                log.Info("程序未能找到配置文件: " + mqConfig + " ,不进行监控");
            }
            else
            {
                WinMonitor.Run();
                //MSMQConfig msmqConfig = null;
                //try
                //{
                //    msmqConfig = XmlHelper.Xml2Entity(mqConfig, new MSMQConfig().GetType()) as MSMQConfig;
                //    log.Info("程序开始mq监控: " + mqConfig);
                //    MSMQMonitor.StartRun(msmqConfig);
                //}
                //catch (Exception ex)
                //{
                //    log.Error("加载msmq过程中出错误：" + ex.ToString());
                //}
            }
        }
    }
}
