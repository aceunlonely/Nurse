using Nurse.Common.CM;
using Nurse.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nurse.Common.Implements
{
    /// <summary>
    /// 磁盘连接器，通过磁盘文件提交心跳信息
    /// </summary>
    public class DiskConnector : IStateCenterConnector
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        internal DiskConnector()
        {
            if (string.IsNullOrEmpty(CommonConfig.DiskStateCenterPath))
            {
                throw new Exception("采用DiskConnector，必须配置节点DiskStateCenterPath");
            }

            try
            {
                if (File.Exists(CommonConfig.DiskStateCenterPath) == false)
                {
                    Directory.CreateDirectory(CommonConfig.DiskStateCenterPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("配置节点DiskStateCenterPath不是一个合理的路径：" + ex.ToString());
            }
        }

        public bool IsCenterAlived()
        {
            //disk 方式，不存在中心存活问题
            return true;
        }

        public DateTime? GetLastBeatTime(string key)
        {
            key = SolveKey(key);
            string strPath = Path.Combine(CommonConfig.DiskStateCenterPath, key);
            if (File.Exists(strPath) == false)
            {
                return null;
            }
            try
            {
                string strContent = File.ReadAllText(strPath, Encoding.UTF8);
                string[] arrStr = strContent.Split(new char[] { '|' });

                DateTime dt;
                if (DateTime.TryParse(arrStr[1], out dt))
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("GetLastBeatTime处理文件:" + strPath + "出错：" + ex.ToString());
                return null;
            }
        }

        public bool Beat(string key, DateTime? beatTime)
        {
            key = SolveKey(key);
            string strPath = Path.Combine(CommonConfig.DiskStateCenterPath, key);
            try
            {
                string strContent = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + (beatTime.HasValue ? beatTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
                File.WriteAllText(strPath, strContent, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("GetLastBeatTime处理文件:" + strPath + "出错：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 某个服务存活状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsClientAlived(string key)
        {
            key = SolveKey(key);
            string strPath = Path.Combine(CommonConfig.DiskStateCenterPath, key);
            if (File.Exists(strPath) == false)
            {
                return false;
            }
            try
            {
                string strContent = File.ReadAllText(strPath, Encoding.UTF8);
                string[] arrStr =strContent.Split(new char[] { '|' });

                DateTime dt;
                if (DateTime.TryParse(arrStr[0], out dt))
                {
                    //超过2分钟，代表客户端无连接
                    if ((DateTime.Now - dt).TotalSeconds > 2 * 60)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                CommonLog.InnerErrorLog.Error("IsClientAlived处理文件:" + strPath + "出错：" + ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// 处理key，剔除特殊字符
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        private string SolveKey(string key)
        {
            return key.Replace(":", "");
        }

        public string SendMsg(string type, string msg)
        {
            // do nothin
            return string.Empty;
        }
    }
}
