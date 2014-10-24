using System.ComponentModel;

namespace TPLib.Log.Data
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum ELogType
    {
        /// <summary>
        /// 调试 警告
        /// </summary>
        [Description("警告")]
        Warn = 1,

        /// <summary>
        /// 调试 信息
        /// </summary>
        [Description("信息")]
        Info = 2,

        /// <summary>
        /// 调试
        /// </summary>
        [Description("调试")]
        Debug = 3,

        /// <summary>
        /// 调试 错误
        /// </summary>
        [Description("错误")]
        Error = 4
    }
}
