using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Args
{
    /// <summary>
    /// 操作信息事件
    /// </summary>
    public class OptLogArgs : EventArgs
    {

        public OptLogData OptData;
        public OptLogArgs(OptLogData optData)
        {
            OptData = optData;
        }

    }



    /// <summary>
    /// 操作Args
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OptLogEvent(object sender, OptLogArgs e);
}
