using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPLib.Log.Config
{
    internal class LogConfigItem
    {
        public LogConfigItem(List<string> mobiles, List<string> mails)
        {
            Mobiles = mobiles;
            Mails = mails;
        }
        public List<string> Mobiles { get; set; }
        public List<string> Mails { get; set; }

    }
}
