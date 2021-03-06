﻿using Nurse.Common.CM;
using Nurse.Common.DDD;
using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStateCenter.DDD;

namespace Nurse.Test
{
    class TestCon
    {
        public void Test()
        {
            //TestGetLocalMac();

            //TestCloseProcess();
            //TestExsitProcess();
            //TestStartProcess();
            //TestService();

            //TestConfig();

            //TestWebStateCenter();

            //TestWebConfig();
            //TestReadWebConfig();
            //TestInvokeYYJKWebapi();
        }

        public void TestInvokeYYJKWebapi()
        {
            /* 
                 * curl -i -X POST -H 'Content-Type:application/json' -d'{
        "provider": "捷通科技",
        "systemCode": "test",
        "name": "aa",
        "hostIp": "192.168.10.229",
        "upTime": "2017-11-01 12:59:20",
        "hostType":1,
        "collectionItems": [
            {
                "itemKey": "文件大小",
                "itemType": 1,
                "itemUnit": 1,
                "itemValue": "30m",
                "collectionTime": "2017-01-01 01:12:22"
            },
            {
                "itemKey": "目录大小",
                "itemType": 1,
                "itemUnit": 1,
                "itemValue": "80",
                "collectionTime": "2017-01-01 01:12:22"
            },
            {
                "itemKey": "目录大小1",
                "itemType": 1,
                "itemUnit": 1,
                "itemValue": "80",
                "collectionTime": "2017-01-01 01:12:22"
            }
        ]
    }'  http://192.168.12.11:28888/api/Collect  
                 */
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            wc.Headers.Add(HttpRequestHeader.Accept, "json");
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            string postString = @"{
        ""provider"": ""捷通科技"",
        ""systemCode"": ""test"",
        ""name"": ""aa"",
        ""hostIp"": ""192.168.10.229"",
        ""upTime"": ""2017-11-01 12:59:20"",
        ""hostType"":1,
        ""collectionItems"": [
            {
                ""itemKey"": ""文件大小"",
                ""itemType"": 1,
                ""itemUnit"": 1,
                ""itemValue"": ""30m"",
                ""collectionTime"": ""2017-01-01 01:12:22""
            },
            {
                ""itemKey"": ""目录大小"",
                ""itemType"": 1,
                ""itemUnit"": 1,
                ""itemValue"": ""80"",
                ""collectionTime"": ""2017-01-01 01:12:22""
            },
            {
                ""itemKey"": ""目录大小1"",
                ""itemType"": 1,
                ""itemUnit"": 1,
                ""itemValue"": ""80"",
                ""collectionTime"": ""2017-01-01 01:12:22""
            }
        ]
    }";

            byte[] postData = Encoding.UTF8.GetBytes(postString);
            Byte[] pageData = wc.UploadData("http://192.168.12.11:28888/api/Collect", "POST", postData);//得到返回字符流 
            string result = Encoding.UTF8.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句 



        }

        public void TestReadWebConfig()
        {
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map.config.demo");
            var entity =XmlHelper.Xml2Entity(strPath, new MapCollection().GetType());

        }

        public void TestWebConfig()
        {
            MapCollection mc = new MapCollection();
            mc.Nodes = new List<Map>();
            mc.Nodes.Add(new Map() { Code = "Nurse.Test.Exe", Name = "护士测试exe" });
            mc.Nodes.Add(new Map() { Code = "Nurse.Test.Service", Name = "护士测试Service" });


            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map.config.demo");
            XmlHelper.Enity2Xml(strPath, mc);
        }

        public void TestWebStateCenter()
        {
            WebClient wc = new WebClient();
            string result = Encoding.Default.GetString(wc.DownloadData(CommonConfig.WebStateCenterUrl + "?op=isAlive"));  //如果获取网站页面采用的是GB2312，则使用这句 
           
            Console.WriteLine(result);

            //beat
            DateTime dt = DateTime.Now;
            string url = CommonConfig.WebStateCenterUrl + "?op=beat&key=test&val=" + EncodeHelper.UrlEncode(dt.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine(Encoding.Default.GetString(wc.DownloadData(url)));
            Console.WriteLine("时间:" + dt.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("服务器时间：" + Encoding.Default.GetString(wc.DownloadData(CommonConfig.WebStateCenterUrl + "?op=getBeatTime&key=test")));
        }


        public void TestConfig()
        {
            ConfigCollection cc = new ConfigCollection();
            cc.Configs = new List<ConfigNode>();
            cc.Configs.Add(new ConfigNode()
            {
                ID = 1,
                AppName = "zjgl2_service",
                AppPath = "",
                AppType = 1,
                GuardInternal = 1000,
                GuardType = 1,
                Handles = new List<HandleNode>(),
                Remark = "证件管理服务"
            });
            cc.Configs.Add(new ConfigNode()
            {
                ID = 2,
                AppName = "test.exe",
                AppPath = "D:/test.exe",
                AppType = 2,
                GuardInternal = 2000,
                GuardType = 2,
                Handles = new List<HandleNode>(),
                Remark = "测试使用"
            });

            cc.Configs[0].Handles.Add(new HandleNode()
            {
                Condition = 1,
                Desc = "服务不可见",
                Plan = 1,
                Count = 3
            });
            cc.Configs[0].Handles.Add(new HandleNode()
            {
                Condition = 2,
                Desc = "心跳停止",
                Plan = 2,
                Count = 5
            });
            cc.Configs[1].Handles.Add(new HandleNode()
            {
                Condition = 1,
                Desc = "服务不可见",
                Plan = 1,
                Count = 3
            });
            cc.Configs[1].Handles.Add(new HandleNode()
            {
                Condition = 2,
                Desc = "心跳停止",
                Plan = 2,
                Count = 5
            });

            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nurse.config.demo");
            XmlHelper.Enity2Xml(strPath, cc);
        }

        public void TestGetLocalMac()
        {

            Console.WriteLine(ComputerInfo.GetMacAddress());
        }

        public void TestCloseProcess()
        {
            ProcessHelper.CloseProcess("Nurse.Slave");
        }

        public void TestExsitProcess()
        {
            Console.WriteLine(ProcessHelper.ExistProcess("Nurse.Slave") ? "有" : "无");

        }
        public void TestStartProcess()
        {
            Console.WriteLine(ProcessHelper.ExistProcess("Nurse.Slave") ? "有" : "无");
            ProcessHelper.CloseProcess("Nurse.Slave");
            ProcessHelper.StartProcess("Nurse.Slave.exe");
            Console.WriteLine(ProcessHelper.ExistProcess("Nurse.Slave") ? "有" : "无");
            ProcessHelper.CloseProcess("Nurse.Slave");
            Console.WriteLine(ProcessHelper.ExistProcess("Nurse.Slave") ? "有" : "无");
        }

        public void TestService()
        {
            ServiceManger sm = new ServiceManger();

            Console.WriteLine(sm.GetServiceValue("Nurse.Master", "State"));
            ////var state = sm.GetServiceValue("HXHSImportFile", "State");
            //if (sm.GetServiceValue("HXHSImportFile", "State").ToString() == ServiceState.Stopped)
            //{
            //    sm.StartService("HXHSImportFile");
            //    Console.WriteLine("开启服务");
            //    Thread.Sleep(1000);
            //    if (sm.GetServiceValue("HXHSImportFile", "State").ToString() == ServiceState.Running)
            //    {

            //        Console.WriteLine("开启成功");
            //    }
            //}
            //else
            //{
            //    sm.StopService("HXHSImportFile");
            //    Console.WriteLine("停止服务");

            //}
            //Console.WriteLine(state);
        }
    }
}
