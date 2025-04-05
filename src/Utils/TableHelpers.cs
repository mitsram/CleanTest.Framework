using Microsoft.Playwright;
using OpenQA.Selenium.BiDi.Modules.BrowsingContext;

namespace CleanTest.Framework.Utils;

public class TableHelpers
{
    public static async Task<int> GetHeaderColumnIndex(ILocator headers, string columnName)
    {
        var columnCount = await headers.CountAsync();
        int columnIndex = -1;
        
        for (int i = 0; i < columnCount; i++)
        {
            var headerText = await headers.Nth(i).InnerTextAsync();
            if (headerText == columnName)
            {
                columnIndex = i;
            }
        }

        return columnIndex;
    }
    
    public static async Task<ILocator?> GetTargetCellByColumnValue(ILocator table, int searchColumnIndex, string searchColumnValue, int targetColumnIndex)
    {
        var rows = table.Locator("tbody tr");
        var rowCount = await rows.CountAsync();
        
        for (int i = 0; i < rowCount; i++)
        {
            Thread.Sleep(500);
            var cellValue = await table.Locator($"tbody tr:nth-child({i + 1}) td:nth-child({searchColumnIndex + 1})").InnerTextAsync();
            if (cellValue == searchColumnValue)
            {
                return table.Locator($"tbody tr:nth-child({i + 1}) td:nth-child({targetColumnIndex + 1})");
            }
        }
        return null;
    }

    public static async Task<ILocator?> GetTargetCell(ILocator table, int searchColumnIndex, string searchColumnValue,
        string tableName = "")
    {
        var rows = table.Locator("tbody tr");
        var rowCount = await rows.CountAsync();
        
        for (int i = 0; i < rowCount; i++)
        {
            Thread.Sleep(500);
            var cellValue = await table.Locator($"tbody tr:nth-child({i + 1}) td:nth-child({searchColumnIndex + 1})").InnerTextAsync();
            if (cellValue == searchColumnValue)
            {
                return table.Locator($"tbody tr:nth-child({i + 1}) td");
            }
        }
        return null;
    }
}