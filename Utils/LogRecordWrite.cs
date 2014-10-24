using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Utils
{
    public static class LogRecordWrite
    { /// <summary>
        /// 写记录日志
        /// </summary>
        /// <param name="log"> </param>
        public static void Write(LogData log)
        {
            string strFilePath = _GetFilePath(log);
            string dataString = _ToXml<LogData>(log);
            FileUtils.WriteFile(strFilePath, dataString);
        }


        public static string[] GetDirectories()
        {
            string strFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogRecordWrite");
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            return Directory.GetDirectories(strFilePath);
        }

        /// <summary>
        /// 错误文件名
        /// </summary>
        private static string _GetFilePath(LogData log)
        {
            string strFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogRecordWrite", log.GetType().Name);
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            return Path.Combine(strFilePath, Guid.NewGuid().ToString() + ".txt");
        }

        private static string _ToXml<T>(T log)
        {
            StringBuilder sb = new StringBuilder();
            Type type = log.GetType();
            PropertyInfo[] propinfos = type.GetProperties();

            sb.AppendLine("<" + log.GetType().Name + ">");
            foreach (PropertyInfo propinfo in propinfos)
            {
                if (propinfo.GetValue(log, null) != null)
                {
                    sb.Append("<");
                    sb.Append(propinfo.Name);
                    sb.Append(">");
                    sb.Append(propinfo.GetValue(log, null));
                    sb.Append("</");
                    sb.Append(propinfo.Name);
                    sb.AppendLine(">");
                }
            }
            sb.AppendLine("</" + log.GetType().Name + ">");
            return sb.ToString();
        }

    }
}
