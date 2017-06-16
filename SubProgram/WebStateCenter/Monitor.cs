using Nurse.Common.helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebStateCenter
{
    public class MonitorExe
    {
        private static DateTime? lastConfigTime = null;
        /// <summary>
        /// 获取MSMQ配置
        /// </summary>
        /// <returns>配置</returns>
        public static string getMSMQConfig()
        {
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config");
            if (File.Exists(strPath))
            {
                return File.ReadAllText(strPath);
            }
            return "";
        }

        /// <summary>
        /// 上一次配置时间
        /// </summary>
        /// <returns></returns>
        public static string getLastConfigTime()
        {
            if (lastConfigTime.HasValue ==false) {
                lastConfigTime = DateTime.Now;
            }
            return lastConfigTime.Value.ToString("yyyy-MM-dd hh:mm:ss");
        }

        /// <summary>
        /// 更新最后配置时间
        /// </summary>
        public static void UpdateLastConfigTime()
        {
            lastConfigTime = DateTime.Now;
            innerMsg.Clear();
        }

        /// <summary>
        /// 清理本地数据
        /// </summary>
        public static void CleanLoaclMsg() {
            innerMsg.Clear(); 
        }


        private readonly static ConcurrentDictionary<string, MonitorResult> innerMsg = new ConcurrentDictionary<string, MonitorResult>();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void addMsg(string msg)
        {
            List<MonitorResult> mrs = new List<MonitorResult>();
            string[] strMrs = msg.Split(new char[] { '|' });
            foreach (string mr in strMrs)
            {
                string[] strMr = mr.Split(new char[] { '~' });
                mrs.Add(new MonitorResult()
                {
                    Domain = strMr[0],
                    CategoryName = strMr[1],
                    CounterName = strMr[2],
                    Instance = strMr[3],
                    Result = strMr[4],
                    Remark = strMr[5]
                });
            }
            foreach (MonitorResult mr in mrs)
            {
                innerMsg.AddOrUpdate(mr.ToString(), mr, (k, v) => mr);
            }
        }


        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public static List<MonitorResult> GetMsg() {
            List<MonitorResult> mrs = innerMsg.Values.ToList<MonitorResult>();
            mrs = mrs.OrderBy(p => p.Domain).ThenBy(p => p.CategoryName).ToList<MonitorResult>();
            mrs.ForEach(p =>
            {
                string map = "";
                switch (p.CounterName) { 
                    case "Outgoing Messages/sec":
                        map = "传出信息数/秒"; 
                        break;
                    case "Incoming Messages/sec":
                        map = "传入信息数/秒";
                        break;
                    case "Messages in Queue":
                        map = "队列深度";
                        break;
                    default:
                        map = p.CounterName;
                        break;
                }
                p.CounterName = map;
            });

            return mrs;
            
        }
    }

}