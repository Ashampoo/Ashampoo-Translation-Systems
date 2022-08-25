using NPOI.SS.UserModel;

namespace Ashampoo.Translation.Systems.Formats.Gengo;

public static class ExcelExtension
{
    /// <summary>
    /// Try to get the string value of the cell at index.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="index"></param>
    /// <returns>The cell value as a string if it exists, otherwise null</returns>
    public static string? TryGetStringCellValue(this IRow row, int index)
    {
        string? result = default;
        try
        {
            result = row.Cells[index].StringCellValue;
        }
        catch (ArgumentOutOfRangeException)
        {
            // ignored
        }

        return result;
    }
}