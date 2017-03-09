using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.helper
{
    /// <summary>
    /// 动态日志对象类
    /// </summary>
    public class DLog : IDLog
    {
        private object obj = new object();

        private ILog _log;
        private ILog Log
        {
            get
            {
                if (_log == null)
                {
                    this.DynamicLogConfig("Default", "Default");
                    _log = log4net.LogManager.GetLogger("Default", "DefaultLog");

                }
                return _log;
            }

        }

        public void Error(string strInfo)
        {
            Log.Error(strInfo);
        }

        public void Debug(string strInfo)
        {
            Log.Debug(strInfo);
        }

        public void Fatal(string strInfo)
        {
            Log.Fatal(strInfo);
        }

        public void Info(string strInfo)
        {
            Log.Info(strInfo);
        }

        public void Warn(string strInfo)
        {
            Log.Warn(strInfo);
        }

        public void Init(string logName, string file)
        {
            this.DynamicLogConfig(logName, file);
            lock (obj)
            {
                this._log = log4net.LogManager.GetLogger(logName, logName);
            }

        }

        /// <summary>
        /// 动态生成日志配置
        /// </summary>
        /// <param name="logName">日志名</param>
        /// <param name="folder">日志</param>
        private void DynamicLogConfig(string logName, string file = "Default")
        {
            lock (obj)
            {
                foreach (var re in log4net.LogManager.GetAllRepositories())
                {
                    if (re.Name == logName)
                    {
                        return;
                    }
                }
                if (string.IsNullOrEmpty(file))
                {
                    file = "Default";
                }
                RollingFileAppender newAppender = new RollingFileAppender();
                newAppender.File = "logs/" + file;
                newAppender.AppendToFile = true;
                newAppender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size;
                newAppender.Encoding = Encoding.UTF8;
                newAppender.StaticLogFileName = false;
                //newAppender.DatePattern = "yyyy-MM-dd.log";
                newAppender.MaxFileSize = 1024*1024 ;
                newAppender.MaxSizeRollBackups = 10;
                log4net.Layout.PatternLayout newLayout = new log4net.Layout.PatternLayout();
                newLayout.ConversionPattern = "[时间：%d{yyy-MM-dd HH:mm:ss}级别：%level 日志内容]：%m %n";
                newLayout.ActivateOptions();
                newAppender.Layout = newLayout;
                log4net.Filter.LevelRangeFilter newLevelFilter = new log4net.Filter.LevelRangeFilter();
                newLevelFilter.LevelMax = log4net.Core.Level.Fatal;
                newLevelFilter.LevelMin = log4net.Core.Level.Info;
                newLevelFilter.ActivateOptions();
                newAppender.AddFilter(newLevelFilter);
                newAppender.ActivateOptions();
                log4net.Repository.ILoggerRepository repository = log4net.LogManager.CreateRepository(logName);
                // 可配置多个
                log4net.Config.BasicConfigurator.Configure(repository, newAppender);
            }
        }
    }
}
