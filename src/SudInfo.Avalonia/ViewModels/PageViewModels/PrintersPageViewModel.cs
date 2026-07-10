namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class PrintersPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public PrintersPageViewModel(
        PrinterService printersService,
        NavigationService navigationService) {
        #region Services Initialization

        _printersService = printersService;
        _navigationService = navigationService;

        #endregion

        EventHandlerClosedWindowDialog = async (s, e) => await LoadPrinters();

        SearchBoxKeyUpCommand = ReactiveCommand.Create((KeyEventArgs keyEventArgs) => {
            if (string.IsNullOrEmpty(SearchText)) {
                Printers = PrintersFromDataBase;
                return;
            }
            if (keyEventArgs.Key != Key.Enter || PrintersFromDataBase == null)
                return;
            Printers = [
                .. PrintersFromDataBase.Where(x => x.Name!.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                                   x.InventarNumber?.Contains(SearchText) == true ||
                                                   x.SerialNumber!.Contains(SearchText) ||
                                                   x.Computer != null &&
                                                   x.Computer.User?.FIO.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) == true)
            ];
        });
    }

    #endregion

    #region Commands

    public ReactiveCommand<KeyEventArgs, Unit> SearchBoxKeyUpCommand { get; set; }

    #endregion

    #region Services

    private readonly PrinterService _printersService;

    private readonly NavigationService _navigationService;

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Printer>? Printers { get; set; }

    private IReadOnlyCollection<Printer>? PrintersFromDataBase { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial Printer? SelectedPrinter { get; set; }

    #endregion

    #region Public Methods

    public async Task CreateExcelTable() {
        if (Printers == null || Printers.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Printers);
    }

    public async Task OpenAddPrinterWindow() {
        await _navigationService.ShowPrinterWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task OpenEditPrinterWindow() {
        if (SelectedPrinter == null)
            return;
        await _navigationService.ShowPrinterWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, SelectedPrinter.Id);
    }

    public async Task RemovePrinter() {
        if (SelectedPrinter == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить принтер?");
        if (dialogResult == ButtonResult.No)
            return;
        var removePrinterResult = await _printersService.Remove(SelectedPrinter.Id);
        if (!removePrinterResult.Success) {
            await DialogService.ShowErrorMessageBox(removePrinterResult.Message);
            return;
        }
        await LoadPrinters();
    }

    public async Task LoadPrinters() {
        Printers = await _printersService.Get();
        PrintersFromDataBase = Printers;
    }

    #endregion
}