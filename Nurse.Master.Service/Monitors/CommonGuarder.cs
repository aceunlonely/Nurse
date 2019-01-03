using Nurse.Common;
using Nurse.Common.CM;
using Nurse.Common.DDD;
using Nurse.Common.helper;
using Nurse.Common.Implements;
using Nurse.Common.Interface;
using Nurse.Master.Service.Executor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Master.Service.Monitors
{
    /// <summary>
    /// 通用守卫
    /// </summary>
    public class CommonGuarder
    {
        private ConfigNode _config;

        private DateTime? LastProcessTime;
        private DateTime? LastStateCenterTime;
        private DateTime? LastBeatTime;


        private AppErrExecutor _appErrExecutor = new AppErrExecutor();
        public CommonGuarder(ConfigNode config)
        {
            _config = config;
        }

        /// <summary>
        /// 执行监控
        /// </summary>
        public void Run()
        {
            Thread thread = new Thread(new ThreadStart(RecycleGuard));
            thread.Start();
        }

        private void RecycleGuard()
        {
            // 创建自己的日志
            IDLog log = new TinyLog();
            log.Init(_config.ID + "_" + _config.AppName, _config.ID + "_" + _config.AppName + "/log");
            int count1 = 0;

            _appErrExecutor.Log = log;

            //监控间隔提醒
            if (_config.GuardInternal < 1500)
            {

                log.Fatal("注意: 配置的监控间隔小于1.5秒，监控非常容易出问题，监控停止！GuardInternal:" + _config.GuardInternal);
                return;
            }
            else if (_config.GuardInternal < 5000)
            {
                log.Warn("注意: 配置的监控间隔小于5秒，可能存在问题！GuardInternal:" + _config.GuardInternal);
            }
           

            while (Common.IsRun)
            {
                Thread.Sleep(_config.GuardInternal);
                log.Info("开始一次守护主流程+++++++++++++++++++++++++++++++++++++++++++++++");
                //进行进程守护 // 不管是哪种守护类型，都需要进行守护
                try
                {

                    if (_config.AppType == (int)Enums.EnumAppType.服务)
                    {
                        ServiceManger sm = new ServiceManger();
                        if (sm.GetServiceValue(_config.AppName, "State").ToString().Equals(ServiceState.Stopped))
                        {
                            log.Error("服务状态已停止，开始处理错误");
                            _appErrExecutor.HandleError(Enums.EnumHandleCondition.服务进程停止, _config, LastProcessTime);
                            continue;
                        }
                        else
                        {
                            log.Info("服务状态正在运行！");
                            LastProcessTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        if (ProcessHelper.ExistProcess(_config.AppName) == false)
                        {
                            log.Error("进程已停止，开始处理错误");
                            _appErrExecutor.HandleError(Enums.EnumHandleCondition.服务进程停止, _config, LastProcessTime);
                            continue;
                        }
                        else
                        {
                            log.Info("进程运行正常!");
                            LastProcessTime = DateTime.Now;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("进程守护 ID[{0}] 名称[{1}] 出错: ", _config.ID, _config.AppName) + ex.ToString());
                    continue;
                }

                if (_config.GuardType == (int)Enums.EnumGuardType.心跳守护)
                {
                    log.Info("开始心跳检测");
                    try
                    {
                        log.Info("开始检测状态中心健康情况");
                        IStateCenterConnector connector = StateCenterConnectorFactory.GetAvailableConnector();
                        if (connector == null || !connector.IsCenterAlived())
                        {
                            log.Error("状态中心无法连接，开始处理错误：");
                            _appErrExecutor.HandleError(Enums.EnumHandleCondition.服务不可见, _config, LastStateCenterTime);
                            continue;
                        }
                        else
                        {
                            log.Info("状态中心连接正常");
                            LastStateCenterTime = DateTime.Now;
                        }
                        string beatName = ComputerInfo.GetMacAddress() + "." + _config.AppName;
                        log.Info(string.Format("开始获取心跳时间：[{0}]", beatName));
                        // 心跳检测
                        DateTime? lastBeatTime = connector.GetLastBeatTime(beatName);
                        if (lastBeatTime.HasValue == false)
                        {
                            log.Error("连接的服务未向中心发出心跳，首次连接不正常，不进行处理");
                            count1++;
                            if (count1 > 100)
                            {
                                log.Error("超过100次，无法抓取到首次心跳反应，监控终止");
                                return;
                            }
                        }
                        else
                        {
                            //首次心跳
                            if (LastBeatTime == null)
                            {
                                LastBeatTime = lastBeatTime;
                                log.Info("首次心跳时间:" + LastBeatTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else
                            {
                                log.Info("远程心跳时间： " + lastBeatTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                                //先判断是链路是否通着，只有通着的时候，才判断是否心跳
                                if (connector.IsClientAlived(beatName))
                                {
                                    if (lastBeatTime.Value == LastBeatTime.Value)
                                    {
                                        log.Error(string.Format("心跳未进行:处理问题  lastBeatTime:" + lastBeatTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                        _appErrExecutor.HandleError(Enums.EnumHandleCondition.服务可见心脏停止跳动, _config, LastBeatTime);
                                    }
                                    else
                                    {
                                        LastBeatTime = lastBeatTime;
                                        log.Info("正常心跳");
                                        //重置状态
                                        _appErrExecutor.ResetState();
                                    }
                                }
                                else
                                {
                                    log.Error("客户端服务未能正常连接到状态中心，心跳监控失效");
                                    continue;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("心跳 ID[{0}] 名称[{1}] 出错: ", _config.ID, _config.AppName) + ex.ToString());
                        continue;
                    }

                }
            }

        }
    }
}
