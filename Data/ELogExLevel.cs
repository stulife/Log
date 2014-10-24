using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TPLib.Log.Data
{
    /// <summary>
    /// 异常级别
    /// </summary>
    public enum ELogExLevel
    {

        /// <summary>
        /// 低
        /// </summary>
        [Description("低")]
        Lower = 1,

        /// <summary>
        /// 中
        /// </summary>
        [Description("中")]
        Middle = 2,

        /// <summary>
        /// 高
        /// </summary>
        [Description("高")]
        Higher = 3,


        /// <summary>
        /// 致命
        /// </summary>
        [Description("致命")]
        Fatal = 4,
    }
}
