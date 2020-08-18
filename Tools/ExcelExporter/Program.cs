using System;

namespace ExcelExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            ExporterBsonConfig bsonExporter = new ExporterBsonConfig();
            bsonExporter.ExportBsonConfig();

            ExporterJsonConifg jsonExporter = new ExporterJsonConifg();
            jsonExporter.ExportJsonConfig();

        }
    }
}
