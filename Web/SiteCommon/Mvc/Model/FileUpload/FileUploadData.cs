using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common.Mvc.Model.FileUpload
{
    /// <summary>
    /// 文件上传数据
    /// </summary>
    public class FileUploadData
    {
        private static readonly IDictionary<string, string> EnumKeyMap = new Dictionary<string, string>
                                                                             {
                                                                                 {"Error", "error"},
                                                                                 {"Message", "message"},
                                                                                 {"Url", "url"},
                                                                                 {"IsSuccessed","IsSuccessed"},
                                                                                 {"FileName","FileName"},
                                                                                 {"OldFileName","OldFileName"},
                                                                                 {"FileSize","FileSize"}
                                                                             };
        

        public IDictionary<string, object> Data { get; private set; }

        public FileUploadData()
        {
            Data = new Dictionary<string, object>
                       {
                           {EnumKeyMap["Error"],1},
                           {EnumKeyMap["Message"],string.Empty},
                           {EnumKeyMap["Url"],string.Empty},
                           {EnumKeyMap["IsSuccessed"],false},
                           {EnumKeyMap["FileName"],string.Empty},
                           {EnumKeyMap["OldFileName"],string.Empty},
                           {EnumKeyMap["FileSize"],0}
                       };
        }

        /// <summary>
        /// 设置文件上传返回数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetData(FileUploadDateKey key, object value)
        {
            if (key == FileUploadDateKey.IsSuccessed)
            {
                Data[EnumKeyMap["Error"]] = ((bool)value)?0:1;   
            }

            Data[EnumKeyMap[key.ToString()]] = value;
        }
    }

    /// <summary>
    /// 文件上传返回数据
    /// </summary>
    public enum FileUploadDateKey
    { 
        IsSuccessed,
        Message,
        Url,
        FileName,
        OldFileName,
        FileSize
    }
}