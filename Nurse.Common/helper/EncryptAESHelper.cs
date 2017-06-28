using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommonHelper.Encrypt
{
    /// <summary>
    /// Rijndael算法
    /// </summary>
    public class EncryptAESHelper
    {



        //默认密钥向量 
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x68, 0x90, 0xAB, 0xCD, 0xEF, 0x72, 0x34, 0x26, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// 生成秘钥
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            SymmetricAlgorithm aes = Rijndael.Create();
            
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key); // ASCIIEncoding.ASCII.GetString(aes.Key);
        }

        /// <summary>
        /// 生成向量
        /// </summary>
        /// <returns></returns>
        public static string GenerateIV()
        {
            SymmetricAlgorithm aes = Rijndael.Create();
            aes.GenerateIV();
            return Convert.ToBase64String(aes.IV);// ASCIIEncoding.ASCII.GetString(aes.IV);
        }

        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="content">明文字符串</param>
        /// <param name="key">密码</param>
        /// <param name="IV">向量</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static string Encrypt(string content, string key, string IV = "")
        {

            byte[] inputByteArray = Encoding.UTF8.GetBytes(content);//得到需要加密的字节数组 
            byte[] cipherBytes = Encrypt(inputByteArray, key, IV);
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="content">明文字符串</param>
        /// <param name="key">密码</param>
        /// <param name="IV">向量</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static byte[] Encrypt(byte[] content, string key, string IV = "")
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            des.Padding = PaddingMode.ISO10126;
            des.Mode = CipherMode.CBC;

            byte[] inputByteArray = content;//得到需要加密的字节数组 
            //设置密钥及密钥向量
            des.Key = Convert.FromBase64String(key);
            if (string.IsNullOrEmpty(IV))
            {
                des.IV = _key1;
            }
            else
            {
                des.IV = Encoding.UTF8.GetBytes(IV);
            }
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                }
            }
            return cipherBytes;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <param name="key">密码</param>
        /// <param name="IV">向量</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string Decrypt(string cipherText, string key, string IV = "")
        {
            byte[] bCipherText = Convert.FromBase64String(cipherText);
            byte[] decryptBytes = Decrypt(bCipherText, key, IV);
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   ///将字符串后尾的'\0'去掉
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <param name="key">密码</param>
        /// <param name="IV">向量</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static byte[] Decrypt(byte[] cipherText, string key, string IV = "")
        {
            byte[] bCipherText = cipherText;
            SymmetricAlgorithm des = Rijndael.Create();
            des.Padding = PaddingMode.ISO10126;
            des.Mode = CipherMode.CBC;
            des.Key = Convert.FromBase64String(key);
            if (string.IsNullOrEmpty(IV))
            {
                des.IV = _key1;
            }
            else
            {
                des.IV = Encoding.UTF8.GetBytes(IV);
            }
            byte[] decryptBytes = new byte[cipherText.Length];
            using (MemoryStream ms = new MemoryStream(bCipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return decryptBytes;   ///将字符串后尾的'\0'去掉
        }

    }
}
