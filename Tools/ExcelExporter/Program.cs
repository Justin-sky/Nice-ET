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

            //生成Json数据， 用于转换flatbuffer
            ExporterJsonConfig jsonExporter = new ExporterJsonConfig();
            jsonExporter.ExportJsonConfig();

        }
    }
}
