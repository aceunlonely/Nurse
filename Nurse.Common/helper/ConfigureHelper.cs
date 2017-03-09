using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.helper
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public class ConfigureHelper
    {

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">关键词</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static string GetConfigureValue(string key, string defaultValue = "")
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">关键词</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static int GetConfigureIntValue(string key, int defaultValue = 0)
        {
            var value = ConfigurationManager.AppSettings[key];

            int intRealValue;

            if (int.TryParse(value, out intRealValue))
            {
                return intRealValue;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">关键词</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static bool GetConfigureBoolValue(string key, bool defaultValue = false)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (defaultValue)
            {
                if (value.ToLower() == "false" || value == "0")
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (value.ToLower() == "true" || value == "1")
                {
                    return true;
                }
                return false;
            }
        }
    }
}
