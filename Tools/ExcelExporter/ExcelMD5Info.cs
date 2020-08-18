using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelExporter
{
    public struct CellInfo
    {
        public string Type;
        public string Name;
        public string Desc;
    }

    public class ExcelMD5Info
    {
        public Dictionary<string, string> fileMD5 = new Dictionary<string, string>();

        public string Get(string fileName)
        {
            string md5 = "";
            this.fileMD5.TryGetValue(fileName, out md5);
            return md5;
        }

        public void Add(string fileName, string md5)
        {
            this.fileMD5[fileName] = md5;
        }
    }
}
