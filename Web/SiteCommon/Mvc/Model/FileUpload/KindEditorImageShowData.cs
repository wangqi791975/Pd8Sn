using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common.Mvc.Model.FileUpload
{
    /// <summary>
    /// Kind Editor 图片浏览数据
    /// </summary>
    public class KindEditorImageData
    {
        private static readonly IDictionary<string, string> EnumKeyMap = new Dictionary<string, string>
                                                                             {
                                                                                 {"MoveUpDirPath", "moveup_dir_path"},
                                                                                 {"CurrentDirPath", "current_dir_path"},
                                                                                 {"CurrentUrl", "current_url"},
                                                                                 {"TotalCount", "total_count"},
                                                                                 {"FileList", "file_list"},
                                                                                 {"IsDir", "is_dir"},
                                                                                 {"HasFile", "has_file"},
                                                                                 {"FileSize", "filesize"},
                                                                                 {"IsPhoto", "is_photo"},
                                                                                 {"FileType", "filetype"},
                                                                                 {"FileName", "filename"},
                                                                                 {"UploadDateTime", "datetime"}
                                                                             };
        

        public IDictionary<string, object> Data { get; private set; }

        public KindEditorImageData()
        {
            Data = new Dictionary<string, object>
                       {
                           {EnumKeyMap["MoveUpDirPath"],string.Empty},
                           {EnumKeyMap["CurrentDirPath"],string.Empty},
                           {EnumKeyMap["CurrentUrl"],string.Empty},
                           {EnumKeyMap["TotalCount"],0.00M},
                           {EnumKeyMap["FileList"],new List<Dictionary<string,object>>()},
                       };
        }

        public void SetShowData(KindEditorImageKey key, object value)
        {
            Data[EnumKeyMap[key.ToString()]] = value;
        }

        public void AddFileData(Dictionary<KindEditorImageFileKey, object> imageFileMap)
        {
            (Data[EnumKeyMap["FileList"]] as List<Dictionary<string, object>>).Add(imageFileMap.ToDictionary(k => EnumKeyMap[k.Key.ToString()], v => v.Value));
        }
    }

    /// <summary>
    /// 图片列表参数
    /// </summary>
    public enum KindEditorImageKey
    {
        /// <summary>
        /// 上一级文件路径
        /// </summary>
        MoveUpDirPath,
        /// <summary>
        /// 当前文件路径
        /// </summary>
        CurrentDirPath,
        /// <summary>
        /// 当前文件URL
        /// </summary>
        CurrentUrl,
        /// <summary>
        /// 文件及文件夹个数
        /// </summary>
        TotalCount
    }

    /// <summary>
    /// 图片文件参数
    /// </summary>
    public enum KindEditorImageFileKey
    {
        /// <summary>
        /// 是否目录
        /// </summary>
        IsDir,
        /// <summary>
        /// 如果是目录则是否包含文件
        /// </summary>
        HasFile,
        /// <summary>
        /// 文件大小
        /// </summary>
        FileSize,
        /// <summary>
        /// 是否图片文件
        /// </summary>
        IsPhoto,
        /// <summary>
        /// 文件类型
        /// </summary>
        FileType,
        /// <summary>
        /// 文件名
        /// </summary>
        FileName,
        /// <summary>
        /// 上传时间
        /// </summary>
        UploadDateTime
    }
}