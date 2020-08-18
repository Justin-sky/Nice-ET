using NPOI.SS.UserModel;

namespace ExcelExporter
{
    public static class ExcelHelper
    {
        public static string GetCellString(ISheet sheet, int i, int j)
        {
            return sheet.GetRow(i)?.GetCell(j)?.ToString() ?? "";
        }

        public static string GetCellString(IRow row, int i)
        {
            return row?.GetCell(i)?.ToString() ?? "";
        }

        public static string GetCellString(ICell cell)
        {
            return cell?.ToString() ?? "";
        }
    }
}
