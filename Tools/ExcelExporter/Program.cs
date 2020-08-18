using System;

namespace ExcelExporter
{
    class Program
    {
        static void Main(string[] args)
        {

            //生成服务器所用配置文件，直接生成到服务器相关目录中
            ExporterBsonConfig bsonExporter = new ExporterBsonConfig();
            bsonExporter.ExportBsonConfig();

            //ExporterJsonConifg jsonExporter = new ExporterJsonConifg();
            //jsonExporter.ExportJsonConfig();

        }
    }
}
