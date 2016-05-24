using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;

namespace Com.Panduo.Web.Common.Mvc.Helper
{
    /// <summary>
    /// 文件上传辅助类
    /// </summary>
    public static class FileUploadHelper
    {  
        public enum SortFileType
        {
            FileName,
            FileSize,
            FileType,
            FileDate
        }

        public static void SortFiles(this string[] filePaths, SortFileType sortFileType = SortFileType.FileName, bool isAsc = true)
        {
            Array.Sort(filePaths, (a, b) =>
            {
                var fileA = new FileInfo(a);
                var fileB = new FileInfo(b);

                var compareResult = 0;
                switch (sortFileType)
                {
                    case SortFileType.FileName:
                        compareResult = fileA.Name.CompareTo(fileB.Name);
                        break;
                    case SortFileType.FileSize:
                        compareResult = fileA.Length.CompareTo(fileB.Length);
                        break;
                    case SortFileType.FileType:
                        compareResult = fileA.Extension.CompareTo(fileB.Extension);
                        break;
                    case SortFileType.FileDate:
                        compareResult = fileA.LastWriteTime.CompareTo(fileB.LastWriteTime);
                        break;
                }

                return isAsc ? compareResult : -compareResult;
            });
        }
        /// <summary>
        /// 上传根路径
        /// </summary>
        public static readonly string RootPath = string.Empty;
        /// <summary>
        /// 单个文件最大字节
        /// </summary>
        public static readonly int MaxFileSize = 2097152;
        /// <summary>
        /// 是否中文
        /// </summary>
        public static readonly bool IsChinese = false;
        private static string FileFolderDate
        {
            get { return DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo); }
        }
        /// <summary>
        /// 各种类型文件允许上传的文件后缀
        /// </summary>
        public static readonly IDictionary<FileUploadType,string> FileUploadExtendMap;
        static FileUploadHelper()
        {
            var configFile = ConfigurationManager.AppSettings["FileUploadConfigPath"];
            if (configFile.StartsWith("~/"))
            {
                configFile = HttpContext.Current.Server.MapPath(configFile);
            }
            var config = XElement.Load(configFile);

            RootPath = (string)config.Element("UploadFilePath");
            MaxFileSize = int.Parse((string)config.Element("UploadMaxFileSize")); 
            IsChinese = bool.Parse((string)config.Element("IsChinese"));
            FileUploadExtendMap = new Dictionary<FileUploadType, string>();
            FileUploadExtendMap.Add(FileUploadType.File,  (string)config.Element("ArrowFiles").Element("File"));
            FileUploadExtendMap.Add(FileUploadType.Image, (string)config.Element("ArrowFiles").Element("Image"));
            FileUploadExtendMap.Add(FileUploadType.Flash, (string)config.Element("ArrowFiles").Element("Flash"));
            FileUploadExtendMap.Add(FileUploadType.Media, (string)config.Element("ArrowFiles").Element("Media"));
        }

        /// <summary>
        /// 获取允许指定类型的文件后缀(逗号分割)
        /// </summary>
        /// <param name="fileUploadType"></param>
        /// <returns></returns>
        public static string GetArrowedFileExtend(FileUploadType fileUploadType = FileUploadType.File)
        {
            if (FileUploadExtendMap.ContainsKey(fileUploadType))
            {
                return FileUploadExtendMap[fileUploadType];
            }

            return FileUploadExtendMap[FileUploadType.File];
        }

        /// <summary>
        /// 根据上传类型名称获取上传类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static FileUploadType? GetFileUploadType(string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                foreach (var item in Enum.GetNames(typeof(FileUploadType)))
                {
                    if (typeName.Equals(item, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return (FileUploadType)Enum.Parse(typeof(FileUploadType), typeName, true);
                    }
                }    
            }

            return null;
        }

        /// <summary>
        /// 判断一个文件名是否制定合法的类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileUploadType"></param>
        /// <returns></returns>
        public static bool IsFileUploadType(string fileName,FileUploadType fileUploadType)
        {
            return FileUploadExtendMap[fileUploadType].Contains(Path.GetExtension(fileName).Replace(".", "").ToLowerInvariant());
        }

        /// <summary>
        /// 获取新的文件名,格式为:yyyyMMdd-Guid
        /// </summary>
        /// <returns></returns>
        public static string GetNewFileName()
        {
            return string.Format("{0}-{1}", FileFolderDate, Guid.NewGuid());
        }

        /// <summary>
        /// 根据旧的文件名获取新的文件名，包括文件扩展名,格式yyyyMMdd-Guid.扩展名
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <returns></returns>
        public static string GetNewFileName(string oldFileName)
        {
            return string.Format("{0}{1}", GetNewFileName(), Path.GetExtension(oldFileName));
        }

        /// <summary>
        /// 根据文件名获取文件所在全路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileFullName(string fileName)
        {
            return Path.Combine(Path.Combine(RootPath, fileName.Split('-')[0]), fileName);
        }

        /// <summary>
        /// 根据文件名安全的获取文件所在全路径,如果文件夹不存在则创建
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSafeFileFullName(string fileName)
        {
            if (!Directory.Exists(RootPath))
            {
                Directory.CreateDirectory(RootPath);
            }

            var path = Path.Combine(RootPath,fileName.Split('-')[0] );
             if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// 保存文件，返回新的文件名(包括路径)
        /// </summary>
        /// <param name="fileBase"></param>
        /// <returns></returns>
        public static string SaveFile(HttpPostedFileBase fileBase)
        {
            var fileName = GetNewFileName(fileBase.FileName);

            var fileFullName = GetSafeFileFullName(fileName);

            fileBase.SaveAs(fileFullName);

            return fileName;
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void RemoveFile(string fileName)
        {
            var fileFullName = GetFileFullName(fileName);
            if (File.Exists(fileFullName))
            {
                File.Delete(fileFullName);
            }
        }

    }

    /// <summary>
    /// 文件上传类型
    /// </summary>
    public enum FileUploadType
    {
        /// <summary>
        /// 普通文件
        /// </summary>
        File,
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// Flash文件
        /// </summary>
        Flash,
        /// <summary>
        /// 媒体文件
        /// </summary>
        Media
    }
}