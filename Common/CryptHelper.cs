using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Com.Panduo.Common
{
    public static class CryptHelper
    {
        #region Md5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="dataIn">待加密数据</param>
        /// <returns></returns>
        public static string ToMd5(this object dataIn)
        {
            return ToMd5(dataIn == null ? string.Empty : dataIn.ToString());
        } 

        /// <summary>
        /// MD5加密--采用默认的密钥
        /// </summary>
        /// <param name="dataIn">待加密字符串</param>
        /// <returns></returns>
        private static string ToMd5(string dataIn)
        {
            var md5String =  System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dataIn, "MD5")??string.Empty;
            return md5String.ToLower();
        } 
        #endregion

        #region 对称加密
        //默认密钥向量
        private static byte[] keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private static string _encryptKey = "foxAdmin";

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDes(string encryptString, string encryptKey)
        {
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                var rgbIV = keys;
                var inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbKey,rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

         public static string EncryptDes(string encryptString)
         {
             return EncryptDes(encryptString, _encryptKey);
         }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDes(string decryptString, string decryptKey)
        {
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                var rgbIv = keys;
                var inputByteArray = Convert.FromBase64String(decryptString);
                var desCryptoServiceProvider = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, desCryptoServiceProvider.CreateDecryptor(rgbKey,rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
         public static string DecryptDes(string decryptString)
         {
             return DecryptDes(decryptString,_encryptKey);
         }
        #endregion
    }
}
