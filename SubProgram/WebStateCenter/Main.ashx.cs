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



            if (string.IsNullOrEmpty(strOp))
            {
                Return(context, "Hello World");
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