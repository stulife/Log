using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TPLib.Log.Utils
{
    public static class LogErrWrite
    {
        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="error">原因</param>
        public static void Write(string error)
        {
            Write(string.Empty, new Exception(error));
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="ex">异常</param>
        public static void Write(Exception ex)
        {
            Write(string.Empty, ex);
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="ex"></param>
        public static void Write(string reason, Exception ex)
        {
            if (ex == null)
            {
                return;
            }
            string strFileName = _GetFilePath();
            var sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("----------------------------------------------------------");
            sb.AppendLine("时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if (!string.IsNullOrEmpty(reason))
            {
                sb.AppendLine("原因：" + reason);
            }
            sb.AppendLine("异常：");
            sb.AppendLine(ex.ToString());
            sb.AppendLine("----------------------------------------------------------");
            sb.AppendLine();
            FileUtils.WriteFile(strFileName, sb.ToString());
        }
    
        /// <summary>
        /// 错误文件名
        /// </summary>
        private static string _GetFilePath()
        {
            string errorFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogErrWrite");
            if (!Directory.Exists(errorFilePath))
            {
                Directory.CreateDirectory(errorFilePath);
            }
            return Path.Combine(errorFilePath, DateTime.Today.ToString("yyyy-MM-dd") + ".txt");
        }

    }
}
