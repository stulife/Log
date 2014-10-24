using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Utils
{
    public static class FileUtils
    {
        private static object LockObject = new object();


        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filepath">路径+文件名</param>
        /// <param name="content">内容</param>
        public static void WriteFile(string filepath, string content)
        {
            try
            {
                lock (LockObject)
                {

                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(content);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filepath"></param>
        public static void DeleteFile(string filepath)
        {
            lock (LockObject)
            {
                if (File.Exists(filepath))
                    File.Delete(filepath);
            }
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string ReadFile(string filepath)
        {
            string content = "";
            lock (LockObject)
            {
                if (File.Exists(filepath))
                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        content = sr.ReadToEnd();
                    }
            }
            return content;
        }


        /// <summary>
        /// 获取日志文件目录
        /// </summary>
        /// <param name="logtag"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFilePath(string logtag,string path)
        {
            string folderPath = Path.Combine(path, logtag, DateTime.Now.ToString("yyyyMMdd"));

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            FileInfo[] list = new DirectoryInfo(folderPath).GetFiles();
            Array.Sort<FileInfo>(list, new FileLastTimeComparer());

            int filecount = list.Length;
            if (filecount == 0)
            {
                filecount = 1;
            }
            else
            {
                string lastFileName = list[list.Length - 1].Name;
                filecount = Convert.ToInt32(lastFileName.Replace("Log_", "").Replace(".txt", ""));

                if (list[list.Length - 1].Length > (1024 * 1024))
                {
                    filecount++;
                }
            }
            return Path.Combine(folderPath, "Log_" + filecount + ".txt");
        }
    }
}
