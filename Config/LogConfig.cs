using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;

namespace TPLib.Log.Config
{

    internal class LogConfig
    {
        private string _appName;

        private string _logPath = Path.Combine(Environment.CurrentDirectory, "Log");
        private bool _isLocalLog;
        private SortedList<string, LogConfigItem> _logCfgs = new SortedList<string, LogConfigItem>();

        private const string APPNAME = "AppName";
        private const string LOGPATH = "LogPath";
        private const string ISWRITELOCAL = "IsWriteLocal";
        private const string REMINDMOBILES = "RemindMobiles";
        private const string REMINDMAILS = "RemindMails";

        public string AppName
        {
            get { return _appName; }
        }


        public string LogPath
        {
            get { return _logPath; }
        }

        public bool IsLocalLog
        {
            get { return _isLocalLog; }
        }

        public SortedList<string, LogConfigItem> LogConfigItems
        {
            get { return _logCfgs; }
        }

        public LogConfig(XmlNode section)
        {
            _initConfig(section);
        }

        private void _initConfig(XmlNode configRoot)
        {
            if (configRoot != null)
            {
                _appName = configRoot.Attributes[APPNAME].Value;

                if (configRoot.Attributes[LOGPATH] != null)
                {
                    _logPath = configRoot.Attributes[LOGPATH].Value;
                }

                if (configRoot.Attributes[ISWRITELOCAL] != null)
                {
                    _isLocalLog = Convert.ToBoolean(configRoot.Attributes[ISWRITELOCAL].Value);
                }

                XmlNodeList nodeList = configRoot.ChildNodes;
                if (nodeList != null)
                {
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        List<string> mobiles = new List<string>();
                        string strMobiles = string.Empty;
                        if (nodeList[i].Attributes[REMINDMOBILES] != null)
                        {
                            strMobiles = nodeList[i].Attributes[REMINDMOBILES].Value;
                        }
                        if (nodeList[i][REMINDMOBILES] != null)
                        {
                            strMobiles = nodeList[i][REMINDMOBILES].InnerText;
                        }
                        if (!string.IsNullOrEmpty(strMobiles))
                        {
                            foreach (var t in strMobiles.Split(','))
                            {
                                mobiles.Add(t);
                            }
                        }

                        List<string> mails = new List<string>();
                        string strMails = string.Empty;
                        if (nodeList[i].Attributes[REMINDMAILS] != null)
                        {
                            strMails = nodeList[i].Attributes[REMINDMAILS].Value;
                        }
                        if (nodeList[i][REMINDMAILS] != null)
                        {
                            strMails = nodeList[i][REMINDMAILS].InnerText;
                        }
                        if (!string.IsNullOrEmpty(strMails))
                        {
                            foreach (var t in strMails.Split(','))
                            {
                                mails.Add(t);
                            }
                        }
                        _logCfgs.Add(nodeList[i].Name.ToLower(), new LogConfigItem(mobiles, mails));
                    }
                }
            }

        }

    }
}
