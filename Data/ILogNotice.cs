using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPLib.Log.Model;

namespace TPLib.Log.Data
{
    /// <summary>
    /// 日志通知接口
    /// </summary>
    public interface ILogNotice
    {
        /// <summary>
        /// 邮件通知
        /// </summary>
        /// <param name="listMail"> </param>
        /// <param name="data"></param>
        void MailNotice(List<string> listMail,LogData data);

        /// <summary>
        /// 短信通知
        /// </summary>
        /// <param name="listMobile"> </param>
        /// <param name="data"></param>
        void SmsNotice(List<string> listMobile,LogData data);
    }
}
