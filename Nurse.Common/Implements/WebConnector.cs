using CommonHelper.Encrypt;
using Nurse.Common.CM;
using Nurse.Common.helper;
using Nurse.Common.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.Implements
{
    /// <summary>
    /// 连接
    /// </summary>
    public class WebConnector : IStateCenterConnector
    {
        public bool IsCenterAlived()
        {
            return Promise("isAlive", "", "") == "1";
        }

        public DateTime? GetLastBeatTime(string key)
        {
            string result = Promise("getBeatTime", key,"");
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            DateTime dt;
            if (DateTime.TryParse(result, out dt))
            {
                return dt;
            }
            else
            {
                CommonLog.InnerErrorLog.Error("时间转换错误：" + result);
                return null;
            }
        }

        public bool Beat(string key, DateTime? beatTime)
        { 
            return string.IsNullOrEmpty(Promise("beat",key,(beatTime.HasValue ? EncodeHelper.UrlEncode(beatTime.Value.ToString("yyyy-MM-dd HH:mm:ss")) : ""))) ==false;
        }


        public bool IsClientAlived(string key)
        {
            return Promise("isKeyAlived", key, "") == "1";
        }


        public string SendMsg(string type, string msg)
        {
            return Promise("sendMsg", type, msg);
        }


        public string Promise(string name, string key, string value)
        {
            WebClient wc = new WebClient();
            string url = string.Empty;
            if (string.IsNullOrEmpty(CommonConfig.WebStateCenterUrl))
            {
                throw new Exception("未配置节点：WebStateCenterUrl");
            }
            try
            {
                string ekey = string.IsNullOrEmpty(key) ? "" : EncodeHelper.UrlEncode(key);
                string eVal = string.IsNullOrEmpty(value) ? "" :   EncodeHelper.UrlEncode(value);
                url = CommonConfig.WebStateCenterUrl + "?op=" + name + "&key=" + ekey + "&val=" + eVal;
                if (CommonConfig.IsEncrypt)
                {
                    url = url + "&en=" + EncodeHelper.UrlEncode(EncryptAESHelper.Encrypt((name + DateTime.Now.ToString("dd")), CommonConfig.EncryptKey));
                }
                Byte[] pageData = wc.DownloadData(url);
                string result = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句 
                return result;
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("访问站点出错:" + url + "  |" + ex.ToString());
                return string.Empty;
            }
            finally
            {
                wc.Dispose();
            }
        }

    }
}
