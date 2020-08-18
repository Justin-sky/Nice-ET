using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MongoDB.Bson;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ExcelExporter
{
    public class ExporterBsonConfig
    {
        private const string ExcelPath = "../Excel";
        private const string ServerConfigPath = "../../../Config/";


        public void ExportBsonConfig()
        {
            try
            {
                ExportAll(ServerConfigPath);

                ExportAllClass(@"../../../Model/Config", "using MongoDB.Bson.Serialization.Attributes;\n\nnamespace NiceET\n{\n");

                Console.WriteLine($"导出服务端配置完成!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public void ExportAllClass(string exportDir, string csHead)
        {
            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx")
                {
                    continue;
                }

                if (Path.GetFileName(filePath).StartsWith("~"))
                {
                    continue;
                }

                ExportClass(filePath, exportDir, csHead);
                Console.WriteLine($"生成{Path.GetFileName(filePath)}类");
            }

     
        }

        private void ExportClass(string fileName, string exportDir, string csHead)
        {
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }

            string protoName = Path.GetFileNameWithoutExtension(fileName);

            string exportPath = Path.Combine(exportDir, $"{protoName}.cs");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                StringBuilder sb = new StringBuilder();
                ISheet sheet = xssfWorkbook.GetSheetAt(0);
                sb.Append(csHead);

                sb.Append($"\t[Config]\n");
                sb.Append($"\tpublic partial class {protoName}Category : ACategory<{protoName}>\n");
                sb.Append("\t{\n");
                sb.Append($"\t\tpublic static {protoName}Category Instance;\n");
                sb.Append($"\t\tpublic {protoName}Category()\n");
                sb.Append("\t\t{\n");
                sb.Append($"\t\t\tInstance = this;\n");
                sb.Append("\t\t}\n");
                sb.Append("\t}\n\n");

                sb.Append($"\tpublic partial class {protoName}: IConfig\n");
                sb.Append("\t{\n");
                sb.Append("\t\t[BsonId]\n");
                sb.Append("\t\tpublic long Id { get; set; }\n");

                int cellCount = sheet.GetRow(0).LastCellNum;

                for (int i = 0; i < cellCount; i++)
                {
                    string fieldDesc = ExcelHelper.GetCellString(sheet, 0, i);

                    if (fieldDesc.StartsWith("#"))
                    {
                        continue;
                    }

                    string fieldName = ExcelHelper.GetCellString(sheet, 1, i);

                    if (fieldName == "Id" || fieldName == "_id")
                    {
                        continue;
                    }

                    string fieldType = ExcelHelper.GetCellString(sheet, 2, i);
                    if (fieldType == "" || fieldName == "")
                    {
                        continue;
                    }

                    sb.Append($"\t\tpublic {fieldType} {fieldName};\n");
                }

                sb.Append("\t}\n");
                sb.Append("}\n");

                sw.Write(sb.ToString());
            }
        }

        private void ExportAll(string exportDir)
        {


            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx")
                {
                    continue;
                }

                if (Path.GetFileName(filePath).StartsWith("~"))
                {
                    continue;
                }

           

                Export(filePath, exportDir);
            }


            Console.WriteLine("所有表导表完成");
      
        }

        private void Export(string fileName, string exportDir)
        {
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }

            string protoName = Path.GetFileNameWithoutExtension(fileName);
            Console.WriteLine($"{protoName}导表开始");
            string exportPath = Path.Combine(exportDir, $"{protoName}.txt");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                sw.WriteLine('[');
                for (int i = 0; i < xssfWorkbook.NumberOfSheets; ++i)
                {
                    ISheet sheet = xssfWorkbook.GetSheetAt(i);
                    ExportSheet(sheet, sw);
                }
                sw.WriteLine(']');
            }

            Console.WriteLine($"{protoName}导表完成");
        }

        private void ExportSheet(ISheet sheet, StreamWriter sw)
        {
            int cellCount = sheet.GetRow(0).LastCellNum;

            CellInfo[] cellInfos = new CellInfo[cellCount];

            for (int i = 0; i < cellCount; i++)
            {
                string fieldDesc = ExcelHelper.GetCellString(sheet, 0, i);
                string fieldName = ExcelHelper.GetCellString(sheet, 1, i);
                string fieldType = ExcelHelper.GetCellString(sheet, 2, i);
                cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Desc = fieldDesc };
            }

            for (int i = 3; i <= sheet.LastRowNum; ++i)
            {
                if (ExcelHelper.GetCellString(sheet, i, 2) == "")
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder();
                IRow row = sheet.GetRow(i);
                for (int j = 0; j < cellCount; ++j)
                {

                    string fieldValue = ExcelHelper.GetCellString(row, j);
                    if (fieldValue == "")
                    {
                        continue;
                    }

                    if (j > 0)
                    {
                        sb.Append(",");
                    }

                    string fieldName = cellInfos[j].Name;

                    if (fieldName == "Id" || fieldName == "_id")
                    {
                        fieldName = "_id";
                        sb.Append($"[{fieldValue}, {{");
                    }

                    string fieldType = cellInfos[j].Type;
                    sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
                }

                sb.Append("}],");
                sw.WriteLine(sb.ToString());
            }
        }

        private static string Convert(string type, string value)
        {
            switch (type)
            {
                case "int[]":
                case "int32[]":
                case "long[]":
                    return $"[{value}]";
                case "string[]":
                    return $"[{value}]";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    return value;
                case "string":
                    return $"\"{value}\"";
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }


    }
}
