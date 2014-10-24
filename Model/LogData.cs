using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;


namespace TPLib.Log.Model
{

    /// <summary>
    ///日志模型基类
    /// </summary>
    [Serializable]
    public class LogData : MarshalByRefObject
    {
        public virtual string ToLogString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = this.GetType();
            PropertyInfo[] propinfos = type.GetProperties();
            sb.AppendLine("<" + type.Name + ">");
            foreach (PropertyInfo propinfo in propinfos)
            {
                if (propinfo.GetValue(this, null) != null)
                {
                    sb.Append("<");
                    sb.Append(propinfo.Name);
                    sb.Append(">");
                    sb.Append(propinfo.GetValue(this, null));
                    sb.Append("</");
                    sb.Append(propinfo.Name);
                    sb.AppendLine(">");
                }
            }
            sb.AppendLine("</" + type.Name + ">");
            return sb.ToString();
        }

        public virtual string ToAlertString()
        {
            string result = "Nothing";
            Type type = this.GetType();
            if (type.BaseType==typeof(ExLogData))
            {
                ExLogData exLog = (ExLogData) this;
                result = exLog.AlertContent;
            }
            return result;
        }

        public static T FromXml<T>(string xmlstring)
        {
            T obj = default(T);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            TextReader tr = new StringReader(xmlstring);
            using (tr)
            {
                obj = (T)xs.Deserialize(tr);
            }
            return obj;
        }

        public virtual string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = this.GetType();
            PropertyInfo[] propinfos = type.GetProperties();
            foreach (PropertyInfo propinfo in propinfos)
            {
                if (propinfo.GetValue(this, null) != null)
                {

                    sb.Append(propinfo.Name);
                    sb.Append("：");
                    sb.Append(propinfo.GetValue(this, null));
                    sb.Append("\r\n");
                }
            }
            return sb.ToString();
        }

    }
}
