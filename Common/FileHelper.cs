using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Com.Panduo.Common
{
    /// <summary>
    /// 文件处理类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 将一个路径字符串拆成目录名和文件名,文件名支持通配符,函数输出一个包含两个字符串的数组
        /// 其中第一个字符串为目录名,第二个字符串为文件名
        /// </summary>
        /// <param name="strPath">路径字符串</param>
        /// <returns>输出的字符串数组</returns>
        public static string[] SplitPattern(this string strPath)
        {
            string strDir = null;
            string strPattern = null;
            int index = strPath.LastIndexOfAny("/\\".ToCharArray());
            if (index > 0)
            {
                strDir = strPath.Substring(0, index) + System.IO.Path.DirectorySeparatorChar;
                strPattern = strPath.Substring(index + 1);
            }
            else
            {
                strDir = null;
                strPattern = strPath;
            }
            if (strPath.IndexOf('*') >= 0 || strPath.IndexOf('?') >= 0)
            {

            }
            else
            {
                if (System.IO.Directory.Exists(strPath))
                {
                    strDir = strPath;
                    strPattern = "*";
                }
            }
            return new string[] { strDir, strPattern };
        }

        /// <summary>
        /// 格式化输出文件字节数
        /// </summary>
        /// <param name="fileSize">文件字节数</param>
        /// <returns>输出的字符串</returns>
        public static string FormatFileSize(this int fileSize)
        {
            byte[] buffer = new byte[30];
            StrFormatByteSizeA(fileSize, buffer, buffer.Length);
            for (int iCount = 0; iCount < buffer.Length; iCount++)
            {
                if (buffer[iCount] == 0)
                    return System.Text.Encoding.GetEncoding(936).GetString(buffer, 0, iCount);
            }
            return null;
        }

        /// <summary>
        /// 从指定文件读取二进制数据,返回获得的字节数组,若文件不存在或读取失败则返回空引用
        /// </summary>
        /// <param name="strFile">文件名</param>
        /// <returns>获得字节数组,若读取失败则返回空引用</returns>
        public static byte[] LoadBinaryFile(string strFile)
        {
            try
            {
                if (strFile != null && System.IO.File.Exists(strFile))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(strFile);
                    if (info.Length == 0)
                        return null;
                    using (System.IO.FileStream myStream = info.Open(System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        byte[] byts = new byte[myStream.Length];
                        myStream.Read(byts, 0, byts.Length);
                        myStream.Close();
                        return byts;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// 向文件保存二进制数据
        /// </summary>
        /// <param name="strFile">文件名</param>
        /// <param name="byts">字节数组</param>
        /// <returns>保存是否成功</returns>
        public static bool SaveBinaryFile(string strFile, byte[] byts)
        {
            try
            {
                if (strFile != null)
                {
                    using (System.IO.FileStream myStream = new System.IO.FileStream(strFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        myStream.Write(byts, 0, byts.Length);
                        myStream.Close();
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 使用GB2312编码格式读取一个文本文件的内容
        /// </summary>
        /// <param name="strFile">文件名</param>
        /// <returns>读取的文件内容，若文件不存在或发生错误则返回空引用</returns>
        public static string LoadAnsiFile(string strFile)
        {
            System.IO.StreamReader myReader = null;
            try
            {
                if (System.IO.File.Exists(strFile))
                {
                    myReader = new System.IO.StreamReader(strFile, System.Text.Encoding.GetEncoding(936));
                    string strText = myReader.ReadToEnd();
                    myReader.Close();
                    return strText;
                }
            }
            catch
            {
                if (myReader != null)
                    myReader.Close();
            }
            return null;
        }

        /// <summary>
        /// 使用GB2312编码格式保存字符串到一个文件中
        /// </summary>
        /// <param name="strFile">文件名</param>
        /// <param name="strText">字符串数据</param>
        /// <returns>操作是否成功</returns>
        public static bool SaveAnsiFile(string strFile, string strText)
        {
            using (System.IO.StreamWriter myWriter = new System.IO.StreamWriter(strFile, false, System.Text.Encoding.GetEncoding(936)))
            {
                myWriter.Write(strText);
                myWriter.Close();
                return true;
            }
        }

        /// <summary>
        /// 检测文件名是否是合法的文件名
        /// </summary>
        /// <param name="strFileName">文件名字符串</param>
        /// <returns>文件名是否合法</returns>
        public static bool CheckFileNameInValidChar(string strFileName)
        {
            if (string.IsNullOrEmpty(strFileName) || strFileName.Length > 255)
                return false;
            // 在Windows操作系统文件名中不可出现的字符列表
            const string InValidChars = "\\/:*?\"<>|";
            // 检测文件名对于Windows操作系统是否合法
            foreach (char c in strFileName)
            {
                if (InValidChars.IndexOf(c) >= 0)
                {
                    return false;
                }
            }//foreach
            return true;
        }

        /// <summary>
        /// 获得文件名的大写形式的扩展名,若没有扩展名则返回空引用
        /// </summary>
        /// <remarks>文件扩展名就是文件名字符串中最后一个斜杠字符(包括/\)
        /// 后面最后一个点号后面的部分</remarks>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件扩展名</returns>
        public static string GetExtension(string strFileName)
        {
            if (!string.IsNullOrEmpty(strFileName))
            {
                int index = strFileName.LastIndexOf('.');
                int index2 = strFileName.LastIndexOfAny("/\\".ToCharArray());
                if (index >= 0 && index > index2)
                {
                    string ext = strFileName.Substring(index + 1).Trim().ToUpper();
                    if (ext.Length > 0)
                        return ext;
                }
            }
            return null;
        }

        /// <summary>
        /// 修正文件夹字符串,保证字符串以文件夹分隔符结尾
        /// </summary>
        /// <param name="strDir">文件夹字符串</param>
        /// <returns>修正后的字符串</returns>
        public static string FixDirectoryName(string strDir)
        {
            if (strDir != null && strDir.Length > 0)
            {
                if (strDir[strDir.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                    strDir = strDir + System.IO.Path.DirectorySeparatorChar;
            }
            return strDir;
        }

        /// <summary>
        /// 获得没有目录和扩展名的简单文件名
        /// </summary>
        /// <param name="strPath">路径名</param>
        /// <returns>简单文件名</returns>
        public static string GetSimpleName(string strPath)
        {
            string strName = System.IO.Path.GetFileName(strPath);
            int index = strName.LastIndexOf('.');
            if (index >= 0)
                return strName.Substring(0, index);
            else
                return strName;
        }

        /// <summary>
        /// 进行文件通配符的判断,支持任意个*和?,字符串匹配不区分大小写
        /// </summary>
        /// <remarks>本函数调用了 SplitAny 函数</remarks>
        /// <param name="fileName">文件名</param>
        /// <param name="matchPattern">包含通配符的字符串</param>
        /// <returns>文件名是否匹配通配符字符串</returns>
        public static bool MatchFileName(string fileName, string matchPattern)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;
            if (fileName != null)
            {
                fileName = System.IO.Path.GetFileName(fileName);
                fileName = fileName.ToUpper();
            }
            if (matchPattern != null)
                matchPattern = matchPattern.ToUpper();

            string[] strItems = SplitAny(matchPattern, "*?");
            if (strItems != null)
            {
                int index = 0;
                for (int iCount = 0; iCount < strItems.Length; iCount++)
                {
                    string strItem = strItems[iCount];
                    if (strItem == "*")
                    {
                        if (iCount == strItems.Length - 1)
                            return true;
                        index = fileName.IndexOf(strItems[iCount + 1], index);
                        if (index < 0)
                            return false;
                    }
                    else if (strItem == "?")
                    {
                        index++;
                    }
                    else if (fileName.Length < index + strItem.Length
                     || fileName.Substring(index, strItem.Length) != strItem)
                    {
                        return false;
                    }
                    else
                    {
                        index += strItem.Length;
                    }
                }//foreach
                return fileName.Length == index;
            }//if
            return true;
        }

        #region 内部私有的成员
        /// <summary>
        /// 依据若干个分隔字符将一个字符串分隔为若干部分,分隔的部分包括分隔字符
        /// </summary>
        /// <remarks>例如字符串"abc*def?hk",若分隔字符为"*?",
        /// 则本函数处理返回的字符串数组为 "abc" , "*" , "def" ,
        ///  "?" , "hk"</remarks>
        /// <param name="strText">要处理的字符串</param>
        /// <param name="Spliter">分隔字符组成的字符串</param>
        /// <returns>分隔后的字符串数组</returns>
        private static string[] SplitAny(string strText, string Spliter)
        {
            if (strText == null || strText.Length == 0 || Spliter == null || Spliter.Length == 0)
                return null;
            System.Collections.ArrayList myList = new System.Collections.ArrayList();
            int LastIndex = 0;
            for (int iCount = 0; iCount < strText.Length; iCount++)
            {
                if (Spliter.IndexOf(strText[iCount]) >= 0)
                {
                    if (iCount > LastIndex)
                        myList.Add(strText.Substring(LastIndex, iCount - LastIndex));
                    myList.Add(strText.Substring(iCount, 1));
                    LastIndex = iCount + 1;
                }
            }
            if (LastIndex < strText.Length)
                myList.Add(strText.Substring(LastIndex));
            if (myList.Count > 0)
                return (string[])myList.ToArray(typeof(string));
            else
                return null;
        }


        /// <summary>
        /// 比较两个字符串是否相等
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        private static bool EqualString(string s1, string s2)
        {
            if (s1 == null && s2 == null)
                return true;
            if (s1 != null && s2 != null)
                return s1 == s2;
            else
                return false;
        }


        [System.Runtime.InteropServices.DllImport("shlwapi.dll")]
        private static extern int StrFormatByteSizeA(int dw, byte[] buf, int bufSize); 

        #endregion

        /// <summary>
        /// 删除目录以及目录下的所有文件
        /// </summary>
        /// <param name="fullPath"></param>
        public static void DeleteDirectoryAndFile(string fullPath)
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true); 
            }
        }

        /// <summary>
        /// 获取FileSteam中的所有字节数组
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static byte[] GetAllBytesFromFileStream(this FileStream fileStream)
        {
            byte[] bytes = null;
            if (!fileStream.IsNullOrEmpty())
            {
                bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
            }
            return bytes;
        }

        /// <summary>
        /// 保存字节数组到文件
        /// </summary>
        /// <param name="filePath">要保存的文件</param>
        /// <param name="bytes">文件内容</param>
        public static void SaveBytesToFile(this string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// 转换为合法的路径，结尾加上了/
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToSafePath(this string path)
        {
            return path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path : path + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// 将文件目录和文件名连接成一个完整的文件路径
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string ToFullPath(this string path,string fileName)
        {
            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// 根据指定文件获取一个随机文件名
        /// </summary>
        /// <param name="fullName">文件名</param>
        /// <returns></returns>
        public static string ToGuidFileName(this string fullName)
        {
            return Guid.NewGuid() + Path.GetExtension(fullName);
        }

        /// <summary>
        /// 获取一个文件的文件夹
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string GetDirectory(this string fullName)
        {
            return Path.GetDirectoryName(fullName);
        }

        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="fullName"></param>
        public static void DeleteFile(this string fullName)
        { 
            if(File.Exists(fullName))
            {
                File.Delete(fullName); 
            }
        }

        /// <summary>
        /// 递归删除一个目录以及目录下的文件
        /// </summary>
        /// <param name="directory"></param>
        public static void DeleteDirectory(this string directory)
        {
            if(Directory.Exists(directory))
            {
                Directory.Delete(directory, true);    
            }
        }

        /// <summary>
        /// 获取一个文件夹以及子文件夹下的所有文件
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IList<string> GetAllFiles(this string directory)
        {
            var fileInfos = new List<string>();
            if(Directory.Exists(directory))
            {
                fileInfos = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList(); 
            }
            return fileInfos;
        }

        /// <summary>
        /// 读取文件所有内容
        /// </summary>
        /// <param name="fullName">文件路径</param>
        /// <param name="encoding">文件编码</param>
        /// <returns></returns>
        public static string ReadAllFileContent(this string fullName, Encoding encoding = null)
        {
            var content = string.Empty;

            if (File.Exists(fullName))
            {
                content = File.ReadAllText(fullName,encoding??Encoding.UTF8);
            }

            return content;
        }
    }
}
