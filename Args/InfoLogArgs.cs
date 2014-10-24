using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Args
{
    /// <summary>
    /// 调试信息事件
    /// </summary>
    public class InfoLogArgs : EventArgs
    {

        public InfoLogData InfoData;
        public InfoLogArgs(InfoLogData infoData)
        {
            InfoData = infoData;
        }

    }

    /// <summary>
    /// 信息Args
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void InfoLogEvent(object sender, InfoLogArgs e);
}
