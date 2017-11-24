using Nurse.Common.CM;
using Nurse.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Nurse.Common.Implements
{
    /// <summary>
    /// yyjk特别适配的连接器，其中只实现beat方法
    /// </summary>
    public class YYJKWebapiConnector : IStateCenterConnector
    {
        public bool IsCenterAlived()
        {
            return true;
        }

        public DateTime? GetLastBeatTime(string key)
        {
            return null ;
        }

        public bool Beat(string key, DateTime? beatTime)
        {
            if (beatTime.HasValue)
            {
                return this.innerPost(key, beatTime.Value);
            }
            return false;
        }

        public bool IsClientAlived(string key)
        {
            return true;
        }

        public string SendMsg(string type, string msg)
        {
            return "";
        }

        public string Promise(string name, string key, string value)
        {
            return "";
        }

        /// <summary>
        /// 内部post
        /// </summary>
        /// <param name="key"></param>
        /// <param name="beatTime"></param>
        public bool innerPost(string key, DateTime beatTime)
        {
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            wc.Headers.Add(HttpRequestHeader.Accept, "json");
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            if (string.IsNullOrEmpty(YYJKConfig.YYJK_WebapiUrl) || string.IsNullOrEmpty(YYJKConfig.YYJK_Provider) || string.IsNullOrEmpty(YYJKConfig.YYJK_SystemCode) ||
                string.IsNullOrEmpty(YYJKConfig.YYJK_MachineCode) || string.IsNullOrEmpty(YYJKConfig.YYJK_CollectItemKey))
            {
                CommonLog.InnerErrorLog.Error("配置项存在问题，请检查是否配置");
                return false;
            }
            string postString = @"{
        ""provider"": """ + YYJKConfig.YYJK_Provider + @""",
        ""systemCode"": """ + YYJKConfig.YYJK_SystemCode + @""",
        ""name"": """ + YYJKConfig.YYJK_MachineCode + @""",
        ""hostIp"": """ + YYJKConfig.YYJK_HostIp + @""",
        ""upTime"": """ + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + @""",
        ""hostType"":1,
        ""collectionItems"": [
            {
                ""itemKey"": """ + YYJKConfig.YYJK_CollectItemKey + @""",
                ""itemType"": 1,
                ""itemUnit"": 1,
                ""itemValue"": """ + key + @""",
                ""collectionTime"": """ + beatTime.ToString("yyyy-MM-dd hh:mm:ss") + @"""
            }
        ]
    }";

            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(postString);
                Byte[] pageData = wc.UploadData(YYJKConfig.YYJK_WebapiUrl, "POST", postData);
                string result = Encoding.UTF8.GetString(pageData);

                if (result.IndexOf("true") > -1)
                {
                    return true;
                }
                else
                {
                    CommonLog.InnerErrorLog.Error("出错:" + postString + " | " + result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("访问webapi出错:" + YYJKConfig.YYJK_WebapiUrl + "  |" + ex.ToString());
                return false;
            }
            finally
            {
                wc.Dispose();
            }

        }
    }
}
