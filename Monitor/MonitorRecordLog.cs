using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using TPLib.Log.Data;
using TPLib.Log.Libary;
using TPLib.Log.Model;
using TPLib.Log.Utils;

namespace TPLib.Log.Monitor
{
    /// <summary>
    /// 监控线程
    /// </summary>
    public class MonitorRecordLog
    {

        //10分种
        private const int TimerInterval = 10 * 60000;
        private System.Threading.Timer _timer;
        private string FilePath;
        private ILog LogOpt;

        public MonitorRecordLog(ILog ilog)
        {
            LogOpt = ilog;
            FilePath = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = Path.Combine(FilePath, "LogRecordWrite");
        }

        public void Start()
        {
            if (_timer == null)
            {
                _timer = new Timer(new TimerCallback(TimerWorkCycle));
                _timer.Change(60000, TimerInterval);
            }
        }

        private void TimerWorkCycle(object state)
        {
            if (_timer == null)
            {
                return;
            }

            _timer.Change(-1, -1);
            try
            {
                ProcessToService();
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogErrWrite.Write(ex);
            }
            finally
            {
                _timer.Change(TimerInterval, TimerInterval);
            }
        }

        private void ProcessToService()
        {
            string[] Directories = LogRecordWrite.GetDirectories();

            foreach (string dir in Directories)
            {
                DirectoryInfo info = new DirectoryInfo(dir);
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    string XmlStr = FileUtils.ReadFile(file);
                    LogData data = new LogData();
                    try
                    {
                        switch (info.Name)
                        {
                            case "OptLogData":
                                data = LogData.FromXml<OptLogData>(XmlStr);
                                LogOpt.OptLog((OptLogData)data);
                                break;
                            case "ExLogData":
                                data = LogData.FromXml<ExLogData>(XmlStr);
                                LogOpt.ExError((ExLogData)data);
                                break;
                            case "DbLogData":
                                data = LogData.FromXml<DbLogData>(XmlStr);
                                LogOpt.DbError((DbLogData)data);
                                break;
                            case "InfoLogData":
                                data = LogData.FromXml<InfoLogData>(XmlStr);
                                LogOpt.Info((InfoLogData)data);
                                break;
                        }
                        FileUtils.DeleteFile(file);
                    }
                    catch (Exception)
                    {
                        break;
                    }

                }
            }
        }
    }
}
