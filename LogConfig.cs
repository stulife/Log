using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPLib.Log
{
    public class LogConfig
    {
        public LogConfig()
        {
            FilePath = AppDomain.CurrentDomain.BaseDirectory;
            IsLocal = false;
        }

        /// <summary>
        /// 是否保存本地文件
        /// </summary>
        public  string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 是否保存本地文件
        /// </summary>
        public  bool IsLocal
        {
            get;
            set;
        }
       
        /// <summary>
        /// 提醒邮件列表
        /// </summary>
        public  List<string> RemindMailList { get; set; }

        /// <summary>
        /// 提醒手机列表
        /// </summary>
        public  List<string> RemindMobileList { get; set; }

        /// <summary>
        /// 是否重发发送异常日志
        /// </summary>
        public bool IsAgainSend { get; set; }
    } 
}
