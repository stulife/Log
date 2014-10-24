using System;
using System.Collections;

namespace TPLib.Log.Model
{

    /// <summary>
    /// 操作日志格式化字符串
    /// </summary>
    public class LogEventFromat
    {
        public LogEventFromat()
        {

        }

        public LogEventFromat(string srvAddr)
        {
            ServerAddr = srvAddr;
        }

        /// <summary>
        /// 地点（服务地址）
        /// </summary>
        public string ServerAddr { get; set; }

        private System.Collections.Queue _queue = new System.Collections.Queue();

        public void Enqueue(DateTime dt, string strEventMemo, string Memo)
        {
            string strMsg = "";
            strMsg = "[时间：" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "]  ";
            strMsg += strEventMemo + "  ";
            strMsg += Memo + "  ";
            _queue.Enqueue(strMsg);
        }

        public void Enqueue(string Memo)
        {
            _queue.Enqueue(Memo + "--" + ServerAddr);
        }

        public Queue GetQueue()
        {
            return _queue;
        }
    }
}
