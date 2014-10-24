using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Args
{
    /// <summary>
    /// 数据库异常事件  
    /// </summary>
    public class DbExArgs : EventArgs
    {
        public DbLogData DbData;
        public DbExArgs(DbLogData dbData)
        {
            DbData = dbData;
        }
    }

    /// <summary>
    /// 异常Args
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DbExEvent(object sender, DbExArgs e);
}

