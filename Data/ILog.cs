using System;
using TPLib.Log.Model;


namespace TPLib.Log.Data
{

    /// <summary>
    /// 日志类型
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="data"></param>
        void Info(InfoLogData data);

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="data"></param>
        void OptLog(OptLogData data);

        /// <summary>
        /// 程序异常
        /// </summary>
        /// <param name="data"></param>
        void ExError(ExLogData data);

        /// <summary>
        /// 数据库异常
        /// </summary>
        /// <param name="data"></param>
        void DbError(DbLogData data);

    }

}
