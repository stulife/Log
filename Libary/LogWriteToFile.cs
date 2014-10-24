using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TPLib.Log.Data;
using TPLib.Log.Model;
using TPLib.Log.Utils;

namespace TPLib.Log.Libary
{
    public class LogWriteToFile : ILogWriteToFile
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="data">数据</param>
        public void WriteLog(string path, LogData data)
        {
            string filepath = FileUtils.GetFilePath(data.GetType().Name, path);
            FileUtils.WriteFile(filepath, data.ToLogString());
        }
    }
}
