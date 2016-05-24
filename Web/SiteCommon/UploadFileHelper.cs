using System;
using System.IO;
using System.Web;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 上传文件处理类
    /// </summary>
    public static class UploadFileHelper
    {
        /// <summary>
        /// 获取附件保存路径
        /// </summary>
        /// <param name="uploadType"></param>
        /// <returns></returns>
        public static string GetUploadFileSavePath(UploadFileType uploadType)
        {
            var savePath = Path.Combine(HttpContext.Current.Server.MapPath("/"), "UploadFile");//站点根目录
            /*switch (uploadType)
            {
                case UploadFileType.ContactUs:
                    savePath = Path.Combine(savePath, "ContactUs", DateTime.Now.ToString("yyyy-MM-dd"));
                    break;
            }*/

            savePath = Path.Combine(savePath, uploadType.ToString(), DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            return savePath;
        }

        /// <summary>
        /// 根据绝对路径获取相对路径
        /// </summary>
        /// <param name="fullpath">绝对路径</param>
        /// <returns></returns>
        public static string GetUploadFileRelatePath(string fullpath)
        {
            var savePath = Path.Combine(HttpContext.Current.Server.MapPath("/"));//站点根目录
            return fullpath.Replace(savePath,"/").Replace(@"\","/");
        }
    }

    public enum UploadFileType
    {
        ContactUs,

        MailLogo,
    }
}
