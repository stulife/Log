using System;
using System.Text.RegularExpressions;
using TPLib.Log.Data;


namespace TPLib.Log.Model
{
    /// <summary>
    /// 程序异常模型
    /// </summary>
    [Serializable]
    public class ExLogData : LogData
    {

        public ExLogData() { }
 

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="errApp">应用程序</param>
        /// <param name="ex">Exception异常</param>
        /// <param name="exLevel">错误级别</param>
        public ExLogData(string errApp,Exception ex,ELogExLevel exLevel)
        {
            _SetExData(errApp, ex, exLevel);
        }

        /// <summary>
        /// 错误应用程序
        /// </summary>
        public string App { get; set; }

        /// <summary>
        /// 错误命名空间
        /// </summary>
        public string ErrNameSpace { get; set; }

        /// <summary>
        /// 错误模块
        /// </summary>
        public string ErrModel { get; set; }

        /// <summary>
        /// 错误方法
        /// </summary>
        public string ErrTag { get; set; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 错误级别
        /// </summary>
        public ELogExLevel Level { get; set; }

        /// <summary>
        /// 提醒内容
        /// </summary>
        public string AlertContent { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="errApp"></param>
        /// <param name="ex"></param>
        /// <param name="eExLevel"></param>
        private void _SetExData(string errApp,Exception ex,ELogExLevel eExLevel)
        {
            this.Content = ex.ToString();
            string strErrInfo = ex.ToString();
            string[] arryEx = strErrInfo.Split('\r');
            int iLength = arryEx.Length;
            string strResult = "";
            while (true)
            {
                if (arryEx[iLength - 1].IndexOf("位置") > 0)
                {
                    strResult = arryEx[iLength - 1];
                    break;
                }

                iLength = iLength - 1;
                if (iLength == 0)
                {
                    break;
                }
            }
            if (!string.IsNullOrEmpty(strResult))
            {
                if (strResult.IndexOf("位置") > 0)
                {
                    int start = strResult.IndexOf("在") + 1;
                    int end = strResult.IndexOf("位置") - start;
                    if (end > start)
                    {
                        strResult = strResult.Substring(start, end).Trim();
                        string[] arrErr = strResult.Split('.');
                        string strMethod = arrErr[arrErr.Length - 1];
                        string strModel = arrErr[arrErr.Length - 2];
                        string strNamespace = "";

                        if (arrErr.Length > 2)
                        {
                            for (int i = 0; i < arrErr.Length - 2; i++)
                            {
                                strNamespace = strNamespace + arrErr[i] + ".";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < arrErr.Length; i++)
                            {
                                strNamespace = strNamespace + arrErr[i] + ".";
                            }
                        }
                        strNamespace = strNamespace.TrimEnd('.');
                        this.Level = eExLevel;
                        this.App = errApp;
                        this.ErrModel = strModel;
                        this.ErrNameSpace = strNamespace;
                        this.ErrTag = strMethod;
                    }
                }
            }
        }
    }
}
