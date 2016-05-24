using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace Com.Panduo.Common
{
    /// <summary>
    /// 图片压缩辅助类
    /// </summary>
    public class ZipHelper
    {
        /// <summary>
        /// 压缩文件或文件夹
        /// </summary>
        /// <param name="source">文件或文件夹</param>
        /// <param name="zipedFile">压缩后文件名称</param>
        /// <returns></returns>
        public static bool Zip(string source, string zipedFile)
        {
            return Zip(source, zipedFile, null);
        }

        /// <summary>
        /// 压缩文件或文件夹
        /// </summary>
        /// <param name="source">文件或文件夹</param>
        /// <param name="zipedFile">压缩后文件名称</param>
        /// <param name="zipPassword">压缩密码</param>
        /// <returns></returns>
        public static bool Zip(string source, string zipedFile, string zipPassword)
        {
            if (Directory.Exists(source))
            {
                return ZipFolder(source, zipedFile, zipPassword);
            }
            else if (File.Exists(source))
            {
                return ZipFile(source, zipedFile, zipPassword);
            }
            return false;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFile">源文件名称</param>
        /// <param name="zipedFile">压缩后文件名称</param>
        /// <returns></returns>
        public static bool ZipFile(string sourceFile, string zipedFile)
        {
            return ZipFile(sourceFile, zipedFile, null);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFile">源文件名称</param>
        /// <param name="zipedFile">压缩后文件名称</param>
        /// <param name="zipPassword">压缩密码</param>
        /// <returns></returns>
        public static bool ZipFile(string sourceFile, string zipedFile, string zipPassword)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(sourceFile))
            {
                throw new System.IO.FileNotFoundException("Cannot find the file." + sourceFile);
            }
            //FileStream fs = null;
            FileStream zipFile = null;
            ZipOutputStream zipStream = null;
            ZipEntry zipEntry = null;

            bool res = true;
            try
            {
                zipFile = File.OpenRead(sourceFile);
                byte[] buffer = new byte[zipFile.Length];
                zipFile.Read(buffer, 0, buffer.Length);
                zipFile.Close();

                zipFile = File.Create(zipedFile);
                zipStream = new ZipOutputStream(zipFile);
                if (!string.IsNullOrEmpty(zipPassword))
                {
                    zipStream.Password = zipPassword;
                }
                zipEntry = new ZipEntry(Path.GetFileName(sourceFile));
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(6);

                zipStream.Write(buffer, 0, buffer.Length);

            }
            catch
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                    zipEntry = null;
                }
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                    zipFile.Dispose();
                }
                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }

        /// <summary>
        /// 递归压缩目录
        /// </summary>
        /// <param name="sourceFolder">源目录</param>
        /// <param name="zipedFile">压缩后文件名称</param>
        /// <returns></returns>
        public static bool ZipFolder(string sourceFolder, string zipedFile)
        {
            return ZipFolder(sourceFolder, zipedFile, null);
        }

        /// <summary>
        /// 递归压缩目录
        /// </summary>
        /// <param name="sourceFolder">源目录</param>
        /// <param name="zipedFile">压缩后文件名称</param>
        /// <param name="zipPassword">压缩密码</param>
        /// <returns></returns>
        public static bool ZipFolder(string sourceFolder, string zipedFile, string zipPassword)
        {
            if (!Directory.Exists(sourceFolder))
            {
                throw new DirectoryNotFoundException("Cannot find the directory." + sourceFolder);
            }

            bool res = true;
            ZipOutputStream s = new ZipOutputStream(File.Create(zipedFile));
            s.SetLevel(6);
            if (!string.IsNullOrEmpty(zipPassword))
            {
                s.Password = zipPassword;
            }

            res = ZipFolder(sourceFolder, s, string.Empty);

            s.Finish();
            s.Close();
            s.Dispose();

            return res;
        }

        /// <summary>
        /// 递归压缩文件夹方法
        /// </summary>
        /// <param name="sourceFolder">源目录</param>
        /// <param name="zipStream"></param>
        /// <param name="parentFolderName"></param>
        /// <returns></returns>
        private static bool ZipFolder(string sourceFolder, ZipOutputStream zipStream, string parentFolderName)
        {
            bool res = true;
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();

            try
            {
                //创建当前文件夹
                string folderName = Path.GetFileName(sourceFolder);
                entry = new ZipEntry(Path.Combine(parentFolderName, folderName + "/")); //加上 “/” 才会当成是文件夹创建
                zipStream.PutNextEntry(entry);
                zipStream.Flush();


                if (folderName.Length > 0)
                {
                    folderName += "/";
                }
                //先压缩文件，再递归压缩文件夹 
                string[] fileNames = Directory.GetFiles(sourceFolder);
                foreach (string file in fileNames)
                {
                    //打开压缩文件
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string fileName = Path.Combine(parentFolderName, folderName + Path.GetFileName(file));
                    entry = new ZipEntry(fileName);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    entry.Crc = crc.Value;

                    zipStream.PutNextEntry(entry);

                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (entry != null)
                {
                    entry = null;
                }
                GC.Collect();
                GC.Collect(1);
            }


            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                if (!ZipFolder(folder, zipStream, Path.Combine(parentFolderName, Path.GetFileName(sourceFolder))))
                {
                    return false;
                }
            }

            return res;
        }

        private static string GetDictoryName(string dictory)
        {
            if (dictory.EndsWith("\\") || dictory.EndsWith("/"))
            {
                return dictory;
            }
            return dictory + "/";
        }





        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <returns>解压是否成功</returns>  
        public static void UnZip(string zipFilePath, string unZipDir)
        {
            if (zipFilePath.IsNullOrEmpty())
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new System.IO.FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("/"))
                unZipDir += "/";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            using (var s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(unZipDir + directoryName);
                    }
                    if (!directoryName.EndsWith("/"))
                        directoryName += "/";
                    if (fileName != String.Empty)
                    {
                        using (var streamWriter = File.Create(unZipDir + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 解压缩类
    /// </summary>
    class UnZipClass
    {
        private byte[] buffer = new byte[2048];
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bufferSize">缓冲区大小</param>
        public UnZipClass(int bufferSize)
        {
            buffer = new byte[bufferSize];
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public UnZipClass()
        {
        }
        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipFilePatah">解压缩文件路径</param>
        public void UnZipFile(string zipFilePath, string unZipFilePatah)
        {
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry zipEntry = null;
                while ((zipEntry = zipStream.GetNextEntry()) != null)
                {
                    string fileName = Path.GetFileName(zipEntry.Name);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (zipEntry.CompressedSize == 0)
                            break;
                        using (FileStream stream = File.Create(unZipFilePatah + fileName))
                        {
                            while (true)
                            {
                                int size = zipStream.Read(buffer, 0, buffer.Length);
                                if (size > 0)
                                {
                                    stream.Write(buffer, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 解压缩目录
        /// </summary>
        /// <param name="zipDirectoryPath">压缩目录路径</param>
        /// <param name="unZipDirecotyPath">解压缩目录路径</param>
        public void UnZipDirectory(string zipDirectoryPath, string unZipDirecotyPath)
        {
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipDirectoryPath)))
            {
                ZipEntry zipEntry = null;
                while ((zipEntry = zipStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(zipEntry.Name);
                    string fileName = Path.GetFileName(zipEntry.Name);
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (zipEntry.CompressedSize == 0)
                            break;
                        if (zipEntry.IsDirectory)
                        {
                            directoryName = Path.GetDirectoryName(unZipDirecotyPath + zipEntry.Name);
                            Directory.CreateDirectory(directoryName);
                        }
                        using (FileStream stream = File.Create(unZipDirecotyPath + zipEntry.Name))
                        {
                            while (true)
                            {
                                int size = zipStream.Read(buffer, 0, buffer.Length);
                                if (size > 0)
                                {
                                    stream.Write(buffer, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
