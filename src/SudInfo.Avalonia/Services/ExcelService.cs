namespace SudInfo.Avalonia.Services;

public static class ExcelService
{
    public async static Task CreateExcelTableFromEntity<T>(IReadOnlyCollection<T> entity) {
        if (App.MainWindow == null)
            return;
        using XLWorkbook wb = new();

        var ws = wb.Worksheets.Add(nameof(entity));
        ws.Cell(1, 1).InsertTable(entity);
        ws.Columns().AdjustToContents();

        var storageProvider = App.MainWindow.StorageProvider;
        if (storageProvider == null)
            return;

        var saveFilePickerOptions = new FilePickerSaveOptions {
            Title = "Выберите путь сохранения",
            SuggestedFileName = "Table.xlsx",
            FileTypeChoices = [
                new("Excel") {
                    Patterns = ["*.xlsx"]
                }
            ]
        };

        var fileResult = await storageProvider.SaveFilePickerAsync(saveFilePickerOptions);
        if (fileResult != null) {
            await using var stream = await fileResult.OpenWriteAsync();
            wb.SaveAs(stream);
        }
    }
}