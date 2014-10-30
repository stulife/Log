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
using System.Collections.Generic;
using TPLib.Log.Config;

namespace TPLib.Log
{
    public class Log
    {
        /// <summary>
        /// 客户端日志路径 path/App/
        /// </summary>
        private string _clientLogPath { get; set; }
        /// <summary>
        /// 日志AppName
        /// </summary>
        private string LogAppName;

        private LogConfigItem _config;

        /// <summary>
        /// 日志操作接口
        /// </summary>
        private ILog _logOperate = null;

        /// <summary>
        /// 日志提醒接口
        /// </summary>
        private ILogNotice _logRemind = null;
        /// <summary>
        /// 写本地文件接口
        /// </summary>
        private ILogWriteToFile _logWrite;

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


        private static SortedList<string, Log> logs = new SortedList<string, Log>();

        public static Log CreateLogInstance(string logType, ILog logOpt, ILogNotice logRemind)
        {
            Log log = null;
            logType = logType.ToLower();
            if (!logs.ContainsKey(logType))
            {
                if (LogConfigHandler.LogConfig.LogConfigItems.ContainsKey(logType))
                {
                    log = new Log(logType,logOpt, logRemind);
                    logs.Add(logType, log);
                }
            }
            else
            {
                log = logs[logType];
            }
            return log;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="logType">日志类型</param>
        /// <param name="logOpt">ILog日志接口</param>
        /// <param name="logRemind">ILogRemind提醒接口</param>
        public Log(string logType, ILog logOpt, ILogNotice logRemind)
        {
            LogAppName = LogConfigHandler.LogConfig.AppName;
            if (LogConfigHandler.LogConfig.LogConfigItems.ContainsKey(logType))
            {
                _config = LogConfigHandler.LogConfig.LogConfigItems[logType];
            }
            if (!string.IsNullOrEmpty(LogConfigHandler.LogConfig.LogPath))
            {
                _clientLogPath = Path.Combine(LogConfigHandler.LogConfig.LogPath, logType);
            }
            _logOperate = logOpt;
            _logRemind = logRemind;
            _logWrite = new LogWriteToFile();
            MonitorRecordLog monitor = new MonitorRecordLog(_logOperate);
            monitor.Start();
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
            log.App = LogAppName;
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
                    Type type = ex.TargetSite.ReflectedType;

                    string strNamespace = "";
                    if (!string.IsNullOrEmpty(type.Namespace))
                    {
                        strNamespace = type.Namespace;
                    }

                    ExLogData log = new ExLogData();
                    log.App = LogAppName;
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
                        _logRemind.MailNotice(_config.Mails, log);
                    }

                    if (elevel == ELogExLevel.Fatal)
                    {
                        if (_logRemind != null)
                        {
                            log.AlertContent = alertInfo;
                            _logRemind.MailNotice(_config.Mails, log);
                            _logRemind.SmsNotice(_config.Mobiles, log);
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
                    log.App = LogAppName;
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
                        _logRemind.MailNotice(_config.Mails, log);
                    }

                    if (eLogExLevel == ELogExLevel.Fatal)
                    {
                        if (_logRemind != null)
                        {
                            log.AlertContent = alertInfo;
                            _logRemind.MailNotice(_config.Mails, log);
                            _logRemind.SmsNotice(_config.Mobiles, log);
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
                _logRemind.MailNotice(_config.Mails, log);
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
                if (LogConfigHandler.LogConfig.IsLocalLog)
                {
                   _logWrite.WriteLog(_clientLogPath, log);
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
                        _logOperate.DbError(dblog);
                        break;
                    case "OptLogData":
                        OptLogData eventlog = log as OptLogData;
                        _logOperate.OptLog(eventlog);
                        break;
                    case "ExLogData":
                        ExLogData exlog = log as ExLogData;
                        _logOperate.ExError(exlog);
                        break;
                    case "InfoLogData":
                        InfoLogData infolog = log as InfoLogData;
                        _logOperate.Info(infolog);
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
