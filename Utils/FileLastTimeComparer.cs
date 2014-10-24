using System.Collections.Generic;
using System.IO;

namespace TPLib.Log.Utils
{
    /// <summary>
    ///文件夹中按时间排序最新的文件读取
    ///</summary>
    public class FileLastTimeComparer : IComparer<FileInfo>
    {
        // IComparer<FileInfo> 成员

        public int Compare(FileInfo x, FileInfo y)
        {
            return x.LastWriteTime.CompareTo(y.LastWriteTime);
        }

    }
}
