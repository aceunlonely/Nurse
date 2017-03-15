using Nurse.Common;
using Nurse.Common.CM;
using Nurse.Common.DDD;
using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Master.Service.Executor
{
    public class AppErrExecutor
    {
        private DLog _log;
        public DLog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = CommonLog.InnerErrorLog;
                }
                return _log;
            }
            set { _log = value; }
        }

        private int Count = 0;

        /// <summary>
        /// 重置状态
        /// </summary>
        public void ResetState()
        {
            Count = 0;
        }

        /// <summary>
        /// 处理错误
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="config">配置</param>
        /// <param name="LastTime">上一次运行时间</param>
        public void HandleError(Enums.EnumHandleCondition condition, ConfigNode config, DateTime? LastTime)
        {
            Enums.EnumHandleCondition cond = condition;
            HandleNode handleNode = null;
            if (config.Handles != null)
            {
                foreach (HandleNode node in config.Handles)
                {
                    if (node.Condition == (int)cond)
                    {
                        handleNode = node;
                        break;
                    }
                }

                if (handleNode == null)
                {
                    Log.Info("未找到处理方案，不处理这种情况");
                }

                switch ((Enums.EnumHandlePlan)handleNode.Plan)
                {
                    case Enums.EnumHandlePlan.停止服务或者进程:
                        CloseNode(config);
                        break;
                    case Enums.EnumHandlePlan.重启服务或者进程:
                        RebootNode(config);
                        break;
                    case Enums.EnumHandlePlan.等待Count满之后重启:
                        if (Count >= handleNode.Count)
                        {
                            Log.Info("开始重启");
                            RebootNode(config);
                            ResetState();
                        }
                        else
                        {
                            Log.Info("重复次数" + Count++ + "未达到处理条件，不做处理");
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 重启节点
        /// </summary>
        /// <param name="node"></param>
        private void RebootNode(ConfigNode node)
        {
            if (node.AppType == (int)Enums.EnumAppType.可执行程序)
            {
                if (ProcessHelper.ExistProcess(node.AppName))
                {
                    Log.Info("先停掉进程" + node.AppName);
                    ProcessHelper.CloseProcess(node.AppName);
                }
                if (ProcessHelper.StartProcess(node.AppPath) == false)
                {
                    Log.Error("重启exe程序失败:" + node.AppName);
                }
                else
                {
                    Log.Info("开启exe程序:" + node.AppPath);
                }
            }
            else
            {
                ServiceManger sm = new ServiceManger();
                if (sm.GetServiceValue(node.AppName, "State").ToString().Equals(ServiceState.Stopped))
                {
                    sm.StartService(node.AppName);

                    Thread.Sleep(500);
                    if (sm.GetServiceValue(node.AppName, "State").ToString().Equals(ServiceState.Running))
                    {
                        Log.Info("重启服务成功:" + node.AppName);
                    }
                    else
                    {
                        Log.Error("重启服务失败:" + node.AppName);
                    }
                }
                else
                {
                    //先停止服务
                    sm.StopService(node.AppName);
                    Log.Info("先关闭服务:" + node.AppName + "  ...");
                    Thread.Sleep(500);
                    Log.Info("  开始开启服务:");
                    sm.StartService(node.AppName);
                    Thread.Sleep(500);
                    if (sm.GetServiceValue(node.AppName, "State").ToString().Equals(ServiceState.Running))
                    {
                        Log.Info("重启服务成功:" + node.AppName);
                    }
                    else
                    {
                        Log.Error("重启服务失败:" + node.AppName);
                    }

                }
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <param name="node"></param>
        private void CloseNode(ConfigNode node)
        {
            if (node.AppType == (int)Enums.EnumAppType.可执行程序)
            {
                if (ProcessHelper.ExistProcess(node.AppName))
                {
                    Log.Info("停掉进程:" + node.AppName);
                    ProcessHelper.CloseProcess(node.AppName);
                }
                else
                {
                    Log.Info("进程:" + node.AppName + " 本停止，不做操作");
                }
            }
            else
            {
                ServiceManger sm = new ServiceManger();
                if (sm.GetServiceValue(node.AppName, "State").ToString().Equals(ServiceState.Running))
                {
                    //先停止服务
                    sm.StopService(node.AppName);
                    Log.Info("关闭服务:" + node.AppName + "  ...");
                    Thread.Sleep(500);
                    if (sm.GetServiceValue(node.AppName, "State").ToString().Equals(ServiceState.Stopped))
                    {
                        Log.Info("关闭服务成功:" + node.AppName);
                    }
                    else
                    {
                        Log.Error("关闭服务失败:" + node.AppName);
                    }
                }
                else
                {
                    Log.Info("服务:" + node.AppName + " 本不在运行，不做操作");
                }
            }
        }
    }
}
