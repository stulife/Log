Log
===
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <!--固定配置-->
  <configSections>
    <section name="Log" type="TPLib.Log.Config.LogConfigHandler,TPLib.Log" />
  </configSections>

  <!--日志配置说明
  Log节点名固定，AppName：当前项目名称， IsLocalLog:是否记录本机日志，LogPath：本机记录日志的地址
  内部节点名是日志类型标记，对应程序中获取日志对象的标记，其中
  RemindMobiles:提醒手机号，RemindMails：提醒邮件
  内部节点可配多个，节点名与日志标记对应
  -->
  <Log AppName="ProjectName"  LogPath="D:\LogTest" IsWriteLocal="true">
    <SystemLog>
      <RemindMobiles>453,123</RemindMobiles>
      <RemindMails>log@log.com,log1@log.com</RemindMails>
    </SystemLog>
    <TradeLog>
      <RemindMobiles>1233</RemindMobiles>
      <RemindMails>log1@log.com,log2@log.com</RemindMails>
    </TradeLog>
  </Log>
</configuration>

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