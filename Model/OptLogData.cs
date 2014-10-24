using System;

namespace TPLib.Log.Model
{
    [Serializable]
    public class OptLogData : LogData
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperateType
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        { get; set; }

        /// <summary>
        /// 事件ID
        /// </summary>
        public string EventGuid { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 0前台，1后台
        /// </summary>
        public int IsCustomer { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string OptTime { get; set; }

    }
}
