using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPLib.Log;
using TPLib.Log.Data;

namespace LogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Log systemLog = Log.CreateLogInstance("systemLog", new SystemLog(), new SystemNotice());
            systemLog.LogInfo("日志内容", TPLib.Log.Data.ELogType.Debug);


            Log TradeLog = Log.CreateLogInstance("TradeLog", null, null);
            TradeLog.LogInfo("日志内容", TPLib.Log.Data.ELogType.Debug);
            Console.ReadLine();
        }


        public class SystemLog : ILog
        {
            public void Info(TPLib.Log.Model.InfoLogData data)
            {
                //记日志 数据库，服务 ，文件等
            }

            public void OptLog(TPLib.Log.Model.OptLogData data)
            {
                //记日志 数据库，服务 ，文件等
            }

            public void ExError(TPLib.Log.Model.ExLogData data)
            {
                //记日志 数据库，服务 ，文件等
            }

            public void DbError(TPLib.Log.Model.DbLogData data)
            {
                //记日志 数据库，服务 ，文件等

            }
        }

        public class SystemNotice : ILogNotice
        {
            public void MailNotice(List<string> listMail, TPLib.Log.Model.LogData data)
            {
                //邮件通知
            }

            public void SmsNotice(List<string> listMobile, TPLib.Log.Model.LogData data)
            {
                //短信提醒
            }
        }
    }
}
