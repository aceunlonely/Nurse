using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebStateCenter.help;

namespace WebStateCenter.DDD
{
    public class MapManager
    {

        private static Dictionary<string, string> _map;
        public static Dictionary<string, string> Map {
            get
            {
                if (_map == null)
                {
                    _map = new Dictionary<string, string>();
                    //加载文件
                    string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map.config");
                    if (File.Exists(strPath))
                    {

                        try
                        {
                            var entity = XmlHelper.Xml2Entity(strPath, new MapCollection().GetType()) as MapCollection;
                            foreach (Map m in entity.Nodes)
                            {
                                _map.Add(m.Code, m.Name);
                            }
                        }
                        catch (Exception ex) { };
                    }
                }
                return _map;
            }
        }
        
        //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map.config.demo");
    }
}