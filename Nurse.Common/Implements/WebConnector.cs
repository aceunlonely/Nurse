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
            WebClient wc = new WebClient();
            if (string.IsNullOrEmpty(CommonConfig.WebStateCenterUrl))
            {
                throw new Exception("未配置节点：WebStateCenterUrl");
            }
            try
            {
                Byte[] pageData = wc.DownloadData(CommonConfig.WebStateCenterUrl + "?op=isAlive");
                string result = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句 


                if (result == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("访问站点出错:" + CommonConfig.WebStateCenterUrl + "?op=isAlive |" + ex.ToString());
                return false;
            }
            finally
            {
                wc.Dispose();
            }
        }

        public DateTime? GetLastBeatTime(string key)
        {
            WebClient wc = new WebClient();
            if (string.IsNullOrEmpty(CommonConfig.WebStateCenterUrl))
            {
                throw new Exception("未配置节点：WebStateCenterUrl");
            }
            try
            {
                Byte[] pageData = wc.DownloadData(CommonConfig.WebStateCenterUrl + "?op=getBeatTime&key=" + EncodeHelper.UrlEncode(key));
                string result = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句 
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
                //return Convert.ToDateTime(result, dtFormat);

            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("访问站点出错:" + CommonConfig.WebStateCenterUrl + "?op=isAlive |" + ex.ToString());
                return null;
            }
            finally
            {
                wc.Dispose();
            }
        }

        public bool Beat(string key, DateTime? beatTime)
        {
            WebClient wc = new WebClient();
            if (string.IsNullOrEmpty(CommonConfig.WebStateCenterUrl))
            {
                throw new Exception("未配置节点：WebStateCenterUrl");
            }

            string strUrl = CommonConfig.WebStateCenterUrl + "?op=beat&key=" + EncodeHelper.UrlEncode(key) + "&val=" + (beatTime.HasValue ? EncodeHelper.UrlEncode(beatTime.Value.ToString("yyyy-MM-dd HH:mm:ss")) : "");
            try
            {
                Byte[] pageData = wc.DownloadData(strUrl);
                string result = Encoding.Default.GetString(pageData);
                if (string.IsNullOrEmpty(result))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("访问站点出错:" + strUrl + " |" + ex.ToString());
                return false;
            }
            finally
            {
                wc.Dispose();
            }
        }


        public bool IsClientAlived(string key)
        {
            WebClient wc = new WebClient();
            if (string.IsNullOrEmpty(CommonConfig.WebStateCenterUrl))
            {
                throw new Exception("未配置节点：WebStateCenterUrl");
            }
            try
            {
                Byte[] pageData = wc.DownloadData(CommonConfig.WebStateCenterUrl + "?op=isKeyAlived&key=" + EncodeHelper.UrlEncode(key));
                string result = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句 


                if (result == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("访问站点出错:" + CommonConfig.WebStateCenterUrl + "?op=isAlive |" + ex.ToString());
                return false;
            }
            finally
            {
                wc.Dispose();
            }
        }
    }
}
