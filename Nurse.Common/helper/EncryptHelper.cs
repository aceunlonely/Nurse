using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dcjet.Framework.Helpers
{
    /// <summary>
    /// �ӽ��ܰ�����
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// Ĭ��������
        /// </summary>
        private const string DefaultPassword = "secretpassword1!";

        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="strValue">������ַ���</param>
        /// <returns>���ؼ���ֵ</returns>
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
        /// DES3 ���ܣ�����Ϊ��
        /// </summary>
        /// <param name="original">���ֵ</param>
        /// <returns>����ֵ</returns>
        public static string EncryptDES(string original)
        {
            return EncryptDES(original, null);
        }

        /// <summary>
        /// DES3 ���ܣ�ָ������
        /// </summary>
        /// <param name="original">���ֵ</param>
        /// <param name="password">����</param>
        /// <returns>����ֵ</returns>
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
        /// DES3 ���ܣ�����Ϊ��
        /// </summary>
        /// <param name="original">���ֵ</param>
        /// <returns>����ֵ</returns>
        public static string DecryptDES(string original)
        {
            return DecryptDES(original, null);
        }

        /// <summary>
        /// DES3 ���ܣ�ָ������
        /// </summary>
        /// <param name="original">���ֵ</param>
        /// <param name="password">����</param>
        /// <returns>����ֵ</returns>
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
