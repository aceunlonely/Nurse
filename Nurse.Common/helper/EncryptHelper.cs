using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dcjet.Framework.Helpers
{
    /// <summary>
    /// 加解密帮助类
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// 默认密码类
        /// </summary>
        private const string DefaultPassword = "secretpassword1!";

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strValue">传入的字符串</param>
        /// <returns>返回加密值</returns>
        public static string GetMD5(string strValue)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] arrData = md5Hasher.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(strValue));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < arrData.Length; i++)
            {
                sBuilder.Append(arrData[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// DES3 加密，密码为空
        /// </summary>
        /// <param name="original">最初值</param>
        /// <returns>返回值</returns>
        public static string EncryptDES(string original)
        {
            return EncryptDES(original, null);
        }

        /// <summary>
        /// DES3 加密，指定密码
        /// </summary>
        /// <param name="original">最初值</param>
        /// <param name="password">密码</param>
        /// <returns>返回值</returns>
        public static string EncryptDES(string original, string password)
        {
            if (string.IsNullOrEmpty(original))
            {
                return "";
            }
            if (string.IsNullOrEmpty(password))
            {
                password = DefaultPassword;
            }
            byte[] buffer1 = new MD5CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(password));
            TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider();
            provider1.Key = buffer1;
            provider1.Mode = CipherMode.ECB;
            byte[] buffer2 = Encoding.Unicode.GetBytes(original);
            string text1 =
                Convert.ToBase64String(provider1.CreateEncryptor().TransformFinalBlock(buffer2, 0, buffer2.Length));
            return text1;
        }

        /// <summary>
        /// DES3 解密，密码为空
        /// </summary>
        /// <param name="original">最初值</param>
        /// <returns>返回值</returns>
        public static string DecryptDES(string original)
        {
            return DecryptDES(original, null);
        }

        /// <summary>
        /// DES3 解密，指定密码
        /// </summary>
        /// <param name="original">最初值</param>
        /// <param name="password">密码</param>
        /// <returns>返回值</returns>
        public static string DecryptDES(string original, string password)
        {
            if (string.IsNullOrEmpty(original))
            {
                return "";
            }
            if (string.IsNullOrEmpty(password))
            {
                password = DefaultPassword;
            }
            byte[] buffer1 = new MD5CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(password));
            TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider();
            provider1.Key = buffer1;
            provider1.Mode = CipherMode.ECB;
            byte[] buffer2 = Convert.FromBase64String(original);
            string text1 =
                Encoding.Unicode.GetString(provider1.CreateDecryptor().TransformFinalBlock(buffer2, 0, buffer2.Length));
            return text1;
        }

        

    }

   
}
