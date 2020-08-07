using System;

namespace ExcelExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            ExporterlHelper helper = new ExporterlHelper();
            helper.ExportServerConfig();
        }
    }
}
