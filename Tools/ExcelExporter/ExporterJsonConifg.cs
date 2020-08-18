using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MongoDB.Bson;
using NiceET;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ExcelExporter
{
    public class ExporterJsonConifg
    {
        private List<string> skillDataFiles = new List<string>()
        {
            "C_SkillData.xlsx",
            "C_Unit.xlsx"
        };

        private const string ExcelPath = "../../../Excel";
        private const string ClientConfigPath = "../../../../Nice-Lua/Assets/AssetsPackage/Config/";

        private bool isClient = false;

        private ExcelMD5Info md5Info;

        public void ExportJsonConfig()
        {
            try
            {
                this.isClient = true;

                ExportAll(ClientConfigPath);

                ExportAllClass(@"../../../../Nice-Lua/Assets/Scripts/Framework/GamePlay/Config");

                Log.Info($"导出服务端配置完成!");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }

        public void ExportAllClass(string exportDir)
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

                if (!skillDataFiles.Contains(Path.GetFileName(filePath)))
                {
                    continue;
                }

                ExportClass(filePath, exportDir);
                Log.Info($"生成{Path.GetFileName(filePath)}类");
            }


        }

        private void ExportClass(string fileName, string exportDir)
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
                sb.Append("using System; \n");
                sb.Append("using System.Collections.Generic; \n");
                sb.Append("using UnityEngine; \n");
                sb.Append("namespace GamePlay { \n");


                //Generate Category
                sb.Append($"\t[Config]\n");
                sb.Append($"\tpublic partial class {protoName}Category : ACategory<{protoName}>\n");
                sb.Append("\t{\n");

                sb.Append($"\t\tpublic static {protoName}Category Instance;\n");
                sb.Append($"\t\tpublic {protoName}Category()\n");
                sb.Append("\t\t{\n");
                sb.Append($"\t\t\tInstance = this;\n");
                sb.Append("\t\t}\n"); //end method

                sb.Append("\t\tpublic override void OnDeserialize(){ \n");
                sb.Append("\t\t\tif (datas != null) \n");
                sb.Append("\t\t\t{ \n");
                sb.Append($"\t\t\t\tdict = new Dictionary<long, {protoName}>(); \n");
                sb.Append("\t\t\t\tforeach (var t in datas) \n");
                sb.Append("\t\t\t\t{\n");
                sb.Append("\t\t\t\t\tdict.Add(t.Id, t); \n");
                sb.Append("\t\t\t\t} \n");//end foreach
                sb.Append("\t\t\t} \n"); //end if
                sb.Append("\t\t}\n \n");  //end method

                sb.Append("\t}\n\n"); //class


                sb.Append("\t[Serializable]\n");
                sb.Append($"\tpublic partial class {protoName}: AConfig\n");
                sb.Append("\t{\n");
                sb.Append("\t\t[SerializeField] public long Id;\n");

                int cellCount = sheet.GetRow(3).LastCellNum;

                for (int i = 2; i < cellCount; i++)
                {
                    string fieldDesc = ExcelHelper.GetCellString(sheet, 2, i);

                    if (fieldDesc.StartsWith("#"))
                    {
                        continue;
                    }

                    // s开头表示这个字段是服务端专用
                    if (fieldDesc.StartsWith("s") && this.isClient)
                    {
                        continue;
                    }

                    string fieldName = ExcelHelper.GetCellString(sheet, 3, i);

                    if (fieldName == "Id" || fieldName == "_id")
                    {
                        continue;
                    }

                    string fieldType = ExcelHelper.GetCellString(sheet, 4, i);
                    if (fieldType == "" || fieldName == "")
                    {
                        continue;
                    }

                    sb.Append($"\t\t[SerializeField] public {fieldType} {fieldName};\n");
                }

                sb.Append("\t}\n");

                sb.Append("}\n"); //End NameSpace

                sw.Write(sb.ToString());
            }
        }

        private void ExportAll(string exportDir)
        {
            string md5File = Path.Combine(ExcelPath, "md5.txt");
            if (!File.Exists(md5File))
            {
                this.md5Info = new ExcelMD5Info();
            }
            else
            {
                this.md5Info = MongoHelper.FromJson<ExcelMD5Info>(File.ReadAllText(md5File));
            }

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

                if (!skillDataFiles.Contains(Path.GetFileName(filePath)))
                {
                    continue;
                }

                string fileName = Path.GetFileName(filePath);
                string oldMD5 = this.md5Info.Get(fileName);
                string md5 = MD5Helper.FileMD5(filePath);
                this.md5Info.Add(fileName, md5);
                // if (md5 == oldMD5)
                // {
                // 	continue;
                // }

                Export(filePath, exportDir);
            }

            File.WriteAllText(md5File, this.md5Info.ToJson());

            Log.Info("所有表导表完成");

        }

        private void Export(string fileName, string exportDir)
        {
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }

            string protoName = Path.GetFileNameWithoutExtension(fileName);
            Log.Info($"{protoName}导表开始");
            string exportPath = Path.Combine(exportDir, $"{protoName}.txt");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                sw.WriteLine("{ \"datas\" : [");
                for (int i = 0; i < xssfWorkbook.NumberOfSheets; ++i)
                {
                    ISheet sheet = xssfWorkbook.GetSheetAt(i);
                    ExportSheet(sheet, sw);
                }
                sw.WriteLine("] }");
            }

            Log.Info($"{protoName}导表完成");
        }

        private void ExportSheet(ISheet sheet, StreamWriter sw)
        {
            int cellCount = sheet.GetRow(3).LastCellNum;

            CellInfo[] cellInfos = new CellInfo[cellCount];

            for (int i = 2; i < cellCount; i++)
            {
                string fieldDesc = ExcelHelper.GetCellString(sheet, 2, i);
                string fieldName = ExcelHelper.GetCellString(sheet, 3, i);
                string fieldType = ExcelHelper.GetCellString(sheet, 4, i);
                cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Desc = fieldDesc };
            }

            for (int i = 5; i <= sheet.LastRowNum; ++i)
            {
                if (ExcelHelper.GetCellString(sheet, i, 2) == "")
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("{ ");

                IRow row = sheet.GetRow(i);
                for (int j = 2; j < cellCount; ++j)
                {
                    string desc = cellInfos[j].Desc.ToLower();
                    if (desc.StartsWith("#"))
                    {
                        continue;
                    }

                    // s开头表示这个字段是服务端专用
                    if (desc.StartsWith("s") && this.isClient)
                    {
                        continue;
                    }

                    // c开头表示这个字段是客户端专用
                    if (desc.StartsWith("c") && !this.isClient)
                    {
                        continue;
                    }

                    string fieldValue = ExcelHelper.GetCellString(row, j);
                    if (fieldValue == "")
                    {
                        continue;
                    }

                    if (j > 2)
                    {
                        sb.Append(",");
                    }

                    string fieldName = cellInfos[j].Name;

                    string fieldType = cellInfos[j].Type;
                    sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
                }

                sb.Append(" },");
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
