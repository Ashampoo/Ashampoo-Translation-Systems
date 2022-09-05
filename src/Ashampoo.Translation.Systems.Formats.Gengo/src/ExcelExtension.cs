using NPOI.SS.UserModel;

namespace Ashampoo.Translation.Systems.Formats.Gengo;

/// <summary>
/// Provides extension methods for working with excel files.
/// </summary>
public static class ExcelExtension
{
    /// <summary>
    /// Try to get the string value of the cell at index.
    /// </summary>
    /// <param name="row">
    /// The row to get the cell from.
    /// </param>
    /// <param name="index">
    /// The zero-based index of the cell.
    /// </param>
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
    
    /// <summary>
    /// Try to get the cell at index.
    /// </summary>
    /// <param name="row">
    /// The row to get the cell from.
    /// </param>
    /// <param name="index">
    /// The zero-based index of the cell.
    /// </param>
    /// <returns>
    /// The <see cref="ICell"/> if it exists, otherwise null.
    /// </returns>
    public static ICell? TryGetCell(this IRow row, int index)
    {
        ICell? result = default;
        try
        {
            result = row.Cells[index];
        }
        catch (ArgumentOutOfRangeException)
        {
            // ignored
        }

        return result;
    }
}