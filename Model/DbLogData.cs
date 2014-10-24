using System;
using System.Text;


namespace TPLib.Log.Model
{
    /// <summary>
    /// 数居库模型
    /// </summary>
   [Serializable]
    public class DbLogData : LogData
    {

        /// <summary>
        /// DB信息
        /// </summary>
        public string DBInfo
        { get; set; }

        /// <summary>
        /// Sql语句
        /// </summary>
        public string Sql
        { get; set; }

        /// <summary>
        /// sql值
        /// </summary>
        public string Value
        { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorInfo
        { get; set; }

        /// <summary>
        /// DB错误代码
        /// </summary>
        public string ErrorNumber
        { get; set; }

    }
}


