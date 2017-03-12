using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.Interface
{
    /// <summary>
    /// 状态中心连接器
    /// </summary>
    public interface IStateCenterConnector
    {
        /// <summary>
        /// 中心是否能连接到
        /// </summary>
        /// <returns></returns>
        bool IsCenterAlived();

        /// <summary>
        /// 获取上一次心跳时间
        /// </summary>
        /// <param name="key">节点名</param>
        /// <returns></returns>
        DateTime? GetLastBeatTime(string key);

        /// <summary>
        /// 心跳方法
        /// </summary>
        /// <param name="key">主机</param>
        /// <param name="beatTime">心跳时间</param>
        /// <returns></returns>
        bool Beat(string key, DateTime? beatTime);

        /// <summary>
        /// 客户端是否连接着
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        bool IsClientAlived(string key);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        string SendMsg(string type,string msg);
    }
}
