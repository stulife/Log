using System;


namespace TPLib.Log.Model
{
    /// <summary>
    /// 调试信息异常
    /// </summary>
      [Serializable]
    public class InfoLogData : LogData
    {

        /// <summary>
        /// 应用程序
        /// </summary>
        public string App { get; set; }


        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace
        { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string NameClass
        { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string NameMethods
        { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public string Level
        { get; set; }
    }
}
