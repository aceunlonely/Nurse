using CommonHelper.Encrypt;
using Nurse.Common.CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStateCenter
{
    /// <summary>
    /// Main 的摘要说明
    /// </summary>
    public class Main : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string strOp = context.Request.QueryString["op"];
            string strKey = context.Request.QueryString["key"];
            string strVal = context.Request.QueryString["val"];
            string strEn = context.Request.QueryString["en"];

            if (string.IsNullOrEmpty(strEn) && CommonConfig.IsEncrypt)
            {
                Return(context, "数据必须进行加密");
                return;
            }
            else if (string.IsNullOrEmpty(strEn))
            {
                //直接继续
            }
            else
            {
                try
                {
                    //存在加密信息时，进行解密处理
                    string strRaw = EncryptAESHelper.Decrypt(strEn, CommonConfig.EncryptKey);
                    if (strRaw != (strOp + DateTime.Now.ToString("dd")))
                    {
                        Return(context, "密钥无法验证通过");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Return(context, "解密出错：" + ex.ToString());
                    return;
                }
            }


            if (string.IsNullOrEmpty(strOp))
            {
                Return(context, "Hello, here is main");
            }
            else
            {
                if (string.IsNullOrEmpty(strOp) == false)
                    strOp = HttpUtility.UrlDecode(strOp);
                if (string.IsNullOrEmpty(strKey) == false)
                    strKey = HttpUtility.UrlDecode(strKey);
                if (string.IsNullOrEmpty(strVal) == false)
                    strVal = HttpUtility.UrlDecode(strVal);
                switch (strOp)
                {
                    case "isAlive":
                        Return(context, "1");
                        break;
                    case "getBeatTime":
                        Return(context, MainExe.GetBeatTime(strKey));
                        break;
                    case "beat":
                        Return(context, MainExe.Beat(strKey, strVal) ? "1" : "0");
                        break;
                    case "isKeyAlived":
                        Return(context, MainExe.IsAlved(strKey) ? "1" : "0");
                        break;
                    case "sendMsg":
                        MainExe.SendMsg(strKey, strVal);
                        Return(context, "1");
                        break;
                    case "getMSMQConfig":
                        Return(context, MonitorExe.getMSMQConfig());
                        break;
                    case "getLastConfigTime":
                        Return(context, MonitorExe.getLastConfigTime());
                        break;
                    case "sendMonitorMsg":
                        MonitorExe.addMsg(strVal);
                        Return(context, "1");
                        break;
                }
            }

        }

        private void Return(HttpContext context, string content)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(content);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}