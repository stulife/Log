using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using TPLib.Log.Args;
using TPLib.Log.Data;
using TPLib.Log.Libary;
using TPLib.Log.Model;
using TPLib.Log.Monitor;
using TPLib.Log.Utils;

namespace TPLib.Log
{
    public class LogUtils
    {

        /// <summary>
        /// 客户端日志路径 path/App/
        /// </summary>
        private string clientLogPath { get; set; }

        /// <summary>
        /// 写本地文件接口（可重写）
        /// </summary>
        private ILogWriteToFile LogWrite ;


        /// <summary>
        /// 日志TAG (ex: TpManage)
        /// </summary>
        public string LogTag
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库日志事件 
        /// </summary>
        public  event DbExEvent DbLoged;

        /// <summary>
        /// 程序异常事件 
        /// </summary>
        public  event ExEvent ExLoged;

        /// <summary>
        /// 调试信息事件 
        /// </summary>
        public  event InfoLogEvent InfoLoged;

        /// <summary>
        /// 操作信息事件 
        /// </summary>
        public  event OptLogEvent OptLoged;

        public LogConfig Config;
      
        /// <summary>
        /// 日志操作接口（可重写）
        /// </summary>
        public ILog LogOperate = null;

        /// <summary>
        /// 日志提醒接口（可重写）
        /// </summary>
        public ILogRemind LogRemind = null;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="logTag">AppName</param>
        /// <param name="config">LogConfig</param>
        /// <param name="logOpt">ILog接口</param>
        public LogUtils(string logTag, LogConfig config, ILog logOpt)
        {
            Config = config;
            LogTag = logTag;
            LogOperate = logOpt;
            LogWrite = new LogWriteToFile();
            clientLogPath = Path.Combine(Config.FilePath, logTag);
            if (Config.IsAgainSend)
            {
                MonitorRecordLog monitor = new MonitorRecordLog(LogOperate);
                monitor.Start();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="logTag">AppName</param>
        /// <param name="config">LogConfig</param>
        /// <param name="logOpt">ILog日志接口</param>
        /// <param name="logRemind">ILogRemind提醒接口</param>
        public LogUtils(string logTag,LogConfig config,ILog logOpt,ILogRemind logRemind)
        {
            Config = config;
            LogTag = logTag;
            LogOperate = logOpt;
            LogRemind = logRemind;
            LogWrite = new LogWriteToFile();
            clientLogPath = Path.Combine(Config.FilePath, logTag);
            if (Config.IsAgainSend)
            {
                MonitorRecordLog monitor = new MonitorRecordLog(LogOperate);
                monitor.Start();
            }
        }

        /// <summary>
        /// 程序调试日志
        /// </summary>
        /// <param name="content">日志信息</param>
        /// <param name="level">级别</param>
        /// <returns></returns>
        public void LogInfo(string content, ELogType level)
        {
            MethodBase method = new StackTrace().GetFrame(1).GetMethod();

            string strNamespace = "";
            if (!string.IsNullOrEmpty(method.ReflectedType.Namespace))
            {
                strNamespace = method.ReflectedType.Namespace; 
            }
            
            InfoLogData log = new InfoLogData();
            log.App = LogTag;
            log.Content = content;
            log.NameSpace = strNamespace;
            log.NameClass = method.ReflectedType.Name;
            log.NameMethods = method.Name;
            log.Level = ((int)level).ToString();

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ThreadWork), log);

            if (InfoLoged != null)
            {
                InfoLoged(null, new InfoLogArgs(log));
            }
        }



        /// <summary>
        /// 程序异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        public void LogExErr(Exception ex)
        {
            LogExErr(ex, ELogExLevel.Lower, "");
        }

        /// <summary>
        /// 程序异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="elevel">级别</param>
        /// <returns></returns>
        public void LogExErr(Exception ex, ELogExLevel elevel)
        {
            LogExErr(ex, elevel, "");
        }

        /// <summary>
        /// 程序异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="elevel">级别</param>
        /// <param name="alertInfo"></param>
        /// <returns></returns>
        public void LogExErr(Exception ex, ELogExLevel elevel,string alertInfo)
        {
            try
            {
                if (ex != null)
                {
                    // ExLogData log = new ExLogData(LogTag, ex, elevel);

                    Type type = ex.TargetSite.ReflectedType;

                    string strNamespace = "";
                    if (!string.IsNullOrEmpty(type.Namespace))
                    {
                        strNamespace = type.Namespace;
                    }

                    ExLogData log = new ExLogData();
                    log.App = LogTag;
                    log.Content = ex.ToString();
                    log.ErrNameSpace = strNamespace;
                    log.ErrModel = type.Name;
                    log.ErrTag = ex.TargetSite.Name;
                    log.Level = elevel;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(_ThreadWork), log);

                    if (ExLoged != null)
                    {
                        ExLoged(null, new ExArgs(log));
                    }

                    if (elevel == ELogExLevel.Higher)
                    {
                        LogRemind.MailRemind(Config.RemindMailList, log);
                    }

                    if (elevel == ELogExLevel.Fatal)
                    {
                        if (LogRemind != null)
                        {
                            log.AlertContent = alertInfo;
                            LogRemind.MailRemind(Config.RemindMailList, log);
                            LogRemind.SmsRemind(Config.RemindMailList, log);
                        }
                    }
                }
            }
            catch (Exception ee)
            {

            }

        }

        public void LogExErr(string strNamespace, string strModel, string strTag, string strErrInfo, ELogExLevel eLogExLevel,string alertInfo)
        {
            try
            {
              

                    ExLogData log = new ExLogData();
                    log.App = LogTag;
                    log.Content = strErrInfo;
                    log.ErrNameSpace = strNamespace;
                    log.ErrModel = strModel;
                    log.ErrTag = strTag;
                    log.Level = eLogExLevel;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(_ThreadWork), log);

                    if (ExLoged != null)
                    {
                        ExLoged(null, new ExArgs(log));
                    }

                    if (eLogExLevel == ELogExLevel.Higher)
                    {
                        LogRemind.MailRemind(Config.RemindMailList, log);
                    }

                    if (eLogExLevel == ELogExLevel.Fatal)
                    {
                        if (LogRemind != null)
                        {
                            log.AlertContent = alertInfo;
                            LogRemind.MailRemind(Config.RemindMailList, log);
                            LogRemind.SmsRemind(Config.RemindMailList, log);
                        }
                    }
               
            }
            catch (Exception ee)
            {

            }

        }

        /// <summary>
        /// 数据库异常
        /// </summary>
        /// <param name="strDbInfo">数据库</param>
        /// <param name="strSql">SQL</param>
        /// <param name="strValue">VALUE</param>
        /// <param name="strErrorInfo">错误信息</param>
        /// <param name="strErrNum">错误代码</param>
        public void LogExErrDb(string strDbInfo, string strSql, string strValue, string strErrorInfo, string strErrNum)
        {
            DbLogData log = new DbLogData
                                {
                                    DBInfo = strDbInfo,
                                    Sql = strSql,
                                    Value = strValue,
                                    ErrorInfo = strErrorInfo
                                };

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ThreadWork), log);

            if (DbLoged != null)
            {
                DbLoged(null, new DbExArgs(log));
            }

            if (strErrNum != "-1000")
            {
                LogRemind.MailRemind(Config.RemindMailList, log);
            }
        }

        #region  操作日志

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="strCreateBy">操作者</param>
        /// <param name="isCustomer">0前台，1后台<</param>
        /// <param name="optModel">事件备注</param>
        /// <returns></returns>
        public void LogOpt(string operateType, string strCreateBy, int isCustomer, LogEventFromat optModel)
        {
            string strMemo = string.Empty;
            Queue queue = optModel.GetQueue();
            if (queue != null && queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    strMemo += queue.Dequeue().ToString();
                    if (queue.Count > 0)
                    {
                        strMemo += "\r\n";
                    }
                }
            }
            if (!string.IsNullOrEmpty(strMemo))
            {
                LogOpt(operateType, strCreateBy, isCustomer, strMemo);
            }

        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param>日志事件ID <name>eventGuid</name> </param>
        /// <param name="strCreateBy">操作者</param>
        /// <param name="isCustomer">0前台，1后台</param>
        /// <param name="strMemo">备注</param>
        /// <returns></returns>
        public void LogOpt(string operateType, string strCreateBy, int isCustomer, string strMemo)
        {
            LogOpt(operateType, Guid.Empty.ToString(), strCreateBy, isCustomer, strMemo);
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="eventGuid">日志事件ID</param>
        /// <param name="strCreateBy">操作者</param>
        /// <param name="isCustomer">0前台，1后台</param>
        /// <param name="strMemo">备注</param>
        /// <returns></returns>
        public void LogOpt(string operateType, string eventGuid, string strCreateBy, int isCustomer, string strMemo)
        {

            OptLogData log = new OptLogData
            {
                OperateType = operateType,
                EventGuid = eventGuid,
                CreateBy = strCreateBy,
                IsCustomer = isCustomer,
                Memo = strMemo,
                OptTime = DateTime.Now.ToString()
            };

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ThreadWork), log);

            if (OptLoged != null)
            {
                OptLoged(null, new OptLogArgs(log));
            }
        }

        #endregion

        private void _ThreadWork(object obj)
        {
            try
            {
                LogData log = (LogData)obj;
                _SendLog(log);
                _WriteLocalLog(log);
            }
            catch (Exception)
            {
               
            }
           
        }

        private void _WriteLocalLog(LogData log)
        {
            try
            {
                if (Config.IsLocal)
                {
                    if (LogWrite != null)
                    {
                        LogWrite.WriteLog(clientLogPath, log);
                    }
                }
            }
            catch (Exception e)
            {
                LogErrWrite.Write(e);
            }
        }

        private void _SendLog(LogData log)
        {
            
            try
            {
                switch (log.GetType().Name)
                {
                    case "DbLogData":
                        DbLogData dblog = log as DbLogData;
                        LogOperate.DbError(dblog);
                        break;
                    case "OptLogData":
                        OptLogData eventlog = log as OptLogData;
                        LogOperate.OptLog(eventlog);
                        break;
                    case "ExLogData":
                        ExLogData exlog = log as ExLogData;
                        LogOperate.ExError(exlog);
                        break;
                    case "InfoLogData":
                        InfoLogData infolog = log as InfoLogData;
                        LogOperate.Info(infolog);
                        break;
                }
            }
            catch (Exception e)
            {
                LogErrWrite.Write(e);
                LogRecordWrite.Write(log);
            }
        }


    }
}
