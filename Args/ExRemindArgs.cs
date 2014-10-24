using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Args
{
    /// <summary>
    /// 程序异常事件 
    /// </summary>
    public class ExRemindArgs : EventArgs
    {

        public ExLogData ExData;
        public ExRemindArgs(ExLogData exData)
        {
            ExData = exData;
        }
    }

    /// <summary>
    /// 异常Args
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ExRemindEvent(object sender, ExArgs e);
}
