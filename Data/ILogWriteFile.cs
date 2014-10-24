using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Data
{
    /// <summary>
    /// 日志客户端
    /// </summary>
    public interface ILogWriteToFile
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="data">日志模型</param>
        void WriteLog(string path,LogData data);
    }
}
