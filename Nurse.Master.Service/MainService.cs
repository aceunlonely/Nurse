﻿using Nurse.Common.CM;
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
            log = new DLog();
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
            // 读取配置
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

        }
    }
}
