using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.helper
{
    public class ServiceManger
    {
        private string strPath;
        private ManagementClass managementClass;

        public ServiceManger()
            : this(".", null, null)
        {
        }

        public ServiceManger(string host, string userName, string password)
        {
            this.strPath = "\\\\" + host + "\\root\\cimv2:Win32_Service";
            this.managementClass = new ManagementClass(strPath);
            if (userName != null && userName.Length > 0)
            {
                ConnectionOptions connectionOptions = new ConnectionOptions();
                //如果连接本地不需要输入用户名和密码
                if (host != ".")
                {
                    connectionOptions.Username = userName;
                    connectionOptions.Password = password;
                    connectionOptions.Authority = "ntlmdomain:DOMAIN";      // 这句很重要
                }
                ManagementScope managementScope = new ManagementScope("\\\\" + host + "\\root\\cimv2", connectionOptions);
                this.managementClass.Scope = managementScope;
            }
        }

        ///// <summary>
        ///// 验证是否能连接到远程计算机 
        ///// </summary>
        ///// <param name="host"></param>
        ///// <param name="userName"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //public static bool RemoteConnectValidate(string host, string userName, string password)
        //{
        //    ConnectionOptions connectionOptions = new ConnectionOptions();
        //    //如果连接本地不需要输入用户名和密码
        //    if (host != ".")
        //    {
        //        connectionOptions.Username = userName;
        //        connectionOptions.Password = password;
        //        connectionOptions.Authority = "ntlmdomain:DOMAIN";      // 这句很重要
        //    }
        //    ManagementScope managementScope = new ManagementScope("\\\\" + host + "\\root\\cimv2", connectionOptions);
        //    try
        //    {
        //        managementScope.Connect();
        //    }
        //    catch
        //    {
        //    }
        //    return managementScope.IsConnected;
        //}

        /// <summary>
        /// 获取指定服务属性的值
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetServiceValue(string serviceName, string propertyName)
        {
            ManagementObject mo = this.managementClass.CreateInstance();
            mo.Path = new ManagementPath(this.strPath + ".Name=\"" + serviceName + "\"");
            return mo[propertyName];
        }

        /// <summary>
        /// 获取所连接的计算机的指定服务数据 
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public ServiceInfo GetServiceList(string serverName)
        {
            return GetServiceList(new string[] {serverName})[0];
        }

        /// <summary>
        /// 获取所连接的计算机的的指定服务数据
        /// </summary>
        /// <param name="serverNames"></param>
        /// <returns></returns>
        public IList<ServiceInfo> GetServiceList(string[] serverNames)
        {
            IList<ServiceInfo> services = new List<ServiceInfo>();
            ManagementObject mo = this.managementClass.CreateInstance();

            ServiceInfo service = null;
            for (int i = 0; i < serverNames.Length; i++)
            {
                mo.Path = new ManagementPath(this.strPath + ".Name=\""+serverNames[i]+"\"");
                service = new ServiceInfo()
                {
                    Name = (string) mo["Name"],
                    DisplayName = (string) mo["DisplayName"],
                    Description = (string) mo["Description"],
                    State = (string) mo["State"],
                    StartMode = (string) mo["StartMode"]
                };
                services.Add(service);
            }
            return services;
        }

        /// <summary>
        /// 启动指定的服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public string StartService(string serviceName)
        {
            string strRst = null;
            ManagementObject mo = this.managementClass.CreateInstance();
            mo.Path = new ManagementPath(this.strPath + ".Name=\"" + serviceName + "\"");
            try
            {
                if ((string) mo["State"] == "Stopped") //!(bool)mo["AcceptStop"] 
                    mo.InvokeMethod("StartService", null);
            }
            catch (ManagementException e)
            {
                strRst = e.Message;
            }
            return strRst;
        }

        /// <summary>
        /// 暂停指定的服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public string PauseService(string serviceName)
        {
            string strRst = null;
            ManagementObject mo = this.managementClass.CreateInstance();
            mo.Path = new ManagementPath(this.strPath + ".Name=\"" + serviceName + "\"");
            try
            {
                //判断是否可以暂停 
                if ((bool) mo["acceptPause"] && (string) mo["State"] == "Running")
                    mo.InvokeMethod("PauseService", null);
            }
            catch (ManagementException e)
            {
                strRst = e.Message;
            }
            return strRst;
        }

        /// <summary>
        /// 恢复指定的服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public string ResumeService(string serviceName)
        {
            string strRst = null;
            ManagementObject mo = this.managementClass.CreateInstance();
            mo.Path = new ManagementPath(this.strPath + ".Name=\"" + serviceName + "\"");
            try
            {
                //判断是否可以恢复 
                if ((bool) mo["acceptPause"] && (string) mo["State"] == "Paused")
                    mo.InvokeMethod("ResumeService", null);
            }
            catch (ManagementException e)
            {
                strRst = e.Message;
            }
            return strRst;
        }

        /// <summary>
        /// 停止指定的服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public string StopService(string serviceName)
        {
            string strRst = null;
            ManagementObject mo = this.managementClass.CreateInstance();
            mo.Path = new ManagementPath(this.strPath + ".Name=\"" + serviceName + "\"");
            try
            {
                //判断是否可以停止 
                if ((bool) mo["AcceptStop"]) //(string)mo["State"]=="Running" 
                    mo.InvokeMethod("StopService", null);
            }
            catch (ManagementException e)
            {
                strRst = e.Message;
            }
            return strRst;
        }
    }

    public class ServiceInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 启动方式
        /// </summary>
        public string StartMode { get; set; }
    }

    /// <summary>
    /// 服务状态
    /// </summary>
    public class ServiceState
    {
        public static string Stopped = "Stopped";

        public static string Running = "Running";

        //public static string 
        //public static string 
    }
}
