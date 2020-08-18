using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MongoDB.Bson;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ExcelExporter
{
    public class ExporterJsonConfig
    {
        private const string ExcelPath = "../Excel";
        private const string JsonPath = "../output_json";

        private List<string> ignoreFiles = new List<string>()
        {
            "StartMachineConfig.xlsx",
            "StartProcessConfig.xlsx",
            "StartSceneConfig.xlsx",
            "StartZoneConfig.xlsx",

        };

        public void ExportJsonConfig()
        {
            try
            {
                ExportAll(JsonPath);

                ExportAllIDLS(@"../output_idl");

                Console.WriteLine($"导出服务端配置完成!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public void ExportAllIDLS(string exportDir)
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

                if (ignoreFiles.Contains(Path.GetFileName(filePath)))
                {
                    continue;
                }

                ExportIDL(filePath, exportDir);
                Console.WriteLine($"生成{Path.GetFileName(filePath)}Schema");
            }

     
        }

        private void ExportIDL(string fileName, string exportDir)
        {
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }

            string protoName = Path.GetFileNameWithoutExtension(fileName).ToLower();

            string exportPath = Path.Combine(exportDir, $"{protoName}.fbs");

            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                StringBuilder sb = new StringBuilder();
                ISheet sheet = xssfWorkbook.GetSheetAt(0);

                sb.Append("namespace fb; \n");

                //gen TB
                sb.Append($"table {protoName}TB\n");
                sb.Append("{\n");
                sb.Append($"\t {protoName}trs:[{protoName}TR];\n");
                sb.Append("}\n\n"); //end TB

                //gen TR
                sb.Append($"table {protoName}TR\n");
                sb.Append("{\n");

                int cellCount = sheet.GetRow(0).LastCellNum;

                for (int i = 0; i < cellCount; i++)
                {
                    string fieldDesc = ExcelHelper.GetCellString(sheet, 0, i);

                    if (fieldDesc.StartsWith("#"))
                    {
                        continue;
                    }

                    string fieldName = ExcelHelper.GetCellString(sheet, 1, i).ToLower();


                    string fieldType = ExcelHelper.GetCellString(sheet, 2, i);
                    if (fieldType == "" || fieldName == "")
                    {
                        continue;
                    }
                    string idlType = Convert(fieldType);

                    sb.Append($"\t {fieldName}:{idlType};\n");
                }

                sb.Append("}\n"); //end TR

                sb.Append($"root_type {protoName}TB;");

                sw.Write(sb.ToString());
            }
        }
        private static string Convert(string type)
        {
            switch (type)
            {
                case "int[]":
                    return "[int]";
                case "int32[]":
                    return "[int32]";
                case "long[]":
                    return "[long]";
                case "string[]":
                    return "[string]";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                case "string":
                    return type;
                default:
                    throw new Exception($"不支持此类型: {type}");
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

                if (ignoreFiles.Contains(Path.GetFileName(filePath)))
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

            string protoName = Path.GetFileNameWithoutExtension(fileName).ToLower();

            Console.WriteLine($"{protoName}导表开始");
            string exportPath = Path.Combine(exportDir, $"{protoName}.txt");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                sw.WriteLine('{');
                sw.WriteLine($"\"{protoName}trs\":[");
                for (int i = 0; i < xssfWorkbook.NumberOfSheets; ++i)
                {
                    ISheet sheet = xssfWorkbook.GetSheetAt(i);
                    ExportSheet(sheet, sw);
                }
                sw.WriteLine("]}");
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
                StringBuilder sb = new StringBuilder();
                if (ExcelHelper.GetCellString(sheet, i, 0) == "")
                {
                    
                    break; ;
                }
                if(i>3)
                    sb.Append(",");

                IRow row = sheet.GetRow(i);
                sb.Append("{");
                for (int j = 0; j < cellCount; ++j)
                {

                    string fieldValue = ExcelHelper.GetCellString(row, j);
                    if (fieldValue == "")
                    {
                        continue;
                    }

                    if (j > 0 )
                    {
                        sb.Append(",");
                    }

                    string fieldName = cellInfos[j].Name.ToLower();

                    string fieldType = cellInfos[j].Type;
                    sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
                }

                sb.Append("}");
          
                
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
