using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;

namespace TPLib.Log.Config
{
    internal class LogConfigHandler : IConfigurationSectionHandler
    {
        static LogConfig _logConfig = null;

        public static LogConfig LogConfig
        {
            get
            {
                if (_logConfig == null)
                {
                    _logConfig = System.Configuration.ConfigurationManager.GetSection("Log") as LogConfig;
                    if (_logConfig == null)
                    {
                        throw new Exception("Log配置错误！");
                    }
                }
                return _logConfig;
            }
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            return new LogConfig(section);
        }


    }

}
