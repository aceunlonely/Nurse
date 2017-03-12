using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebStateCenter.DDD;

namespace WebStateCenter
{
    public class MainExe
    {
        private readonly static ConcurrentDictionary<string, DateTime> dic = new ConcurrentDictionary<string, DateTime>();

        private readonly static ConcurrentDictionary<string, DateTime> innerBeatdic = new ConcurrentDictionary<string, DateTime>();

        private readonly static ConcurrentDictionary<string, string> innerMsg = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="val">值</param>
        public static void SendMsg(string type, string val)
        {
            innerMsg.AddOrUpdate(type, val, (k, v) => val);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetMsg(string type)
        {
            if (innerMsg.ContainsKey(type))
            {
                return innerMsg[type];
            }
            return string.Empty;
        }

        public static List<ServiceState> GetServiceState()
        {
            List<ServiceState> arrR = new List<ServiceState>();

            foreach (string key in dic.Keys)
            {
                string strServerMac = "";
                string strName = key;
                foreach (string code in MapManager.Map.Keys)
                {
                    if (key.EndsWith(code))
                    {
                        int length = code.Length;
                        strServerMac = key.Substring(0, key.Length - length - 1);
                        strName = MapManager.Map[code];
                        break;
                    }
                }
                arrR.Add(new ServiceState()
                {
                    ServerMac = strServerMac,
                    Name = strName,
                    LastBeatTime = GetBeatTime(key),
                    LinkState = IsAlved(key) ? "连接" : "断开"
                });
            }
            return arrR;
        }

        /// <summary>
        /// 获取心跳时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetBeatTime(string key)
        {
            if (dic.ContainsKey(key) == false)
            {
                return string.Empty;
            }
            return dic[key].ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool Beat(string key, string time)
        {
            DateTime now = DateTime.Now;
            //此dic 记录客户端心跳连接是否正常
            innerBeatdic.AddOrUpdate(key, now, (k, v) => now);
            //time 空值时，不心跳
            if (string.IsNullOrEmpty(time))
            {
                return false;
            }
            DateTime dt;
            if (DateTime.TryParse(time, out dt))
            {
                dic.AddOrUpdate(key, dt, (k, v) => dt);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 某个连接是否通着
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsAlved(string key)
        {
            //无key时，直接任务是没有或者
            if (innerBeatdic.ContainsKey(key) == false)
            {
                return false;
            }
            //超过60*2 没有通信， 任务 客户端已经不连接
            if ((DateTime.Now - innerBeatdic[key]).TotalSeconds > 2 * 60)
            {
                return false;
            }
            return true;
        }


    }


    public class ServiceState
    {
        public string ServerMac { get; set; }

        public string Name { get; set; }

        public string LastBeatTime { get; set; }

        public string LinkState { get; set; }
    }
}