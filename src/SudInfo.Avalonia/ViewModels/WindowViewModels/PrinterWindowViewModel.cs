namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class PrinterWindowViewModel : BaseViewModel
{
    #region Services

    private readonly PrinterService _printersService;

    private readonly ComputerService _computerService;

    #endregion

    #region Properties

    [Reactive]
    public partial Printer Printer { get; set; } = new();

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить принтер";

    [Reactive]
    public partial bool IsButtonVisible { get; set; }

    [Reactive]
    public partial bool IsComputer { get; set; }

    #endregion

    #region Public Methods

    public async void Initialization(WindowType windowType, Action close, int? id = null) {
        _windowType = windowType;
        _closedWindow = close;

        if (windowType != WindowType.View)
            IsButtonVisible = true;
        if (id != null) {
            if (windowType != WindowType.View) {
                SaveButtonText = "Сохранить принтер";
            }
            var printerResult = await _printersService.Get(id.GetValueOrDefault());
            if (!printerResult.Success) {
                await DialogService.ShowErrorMessageBox(printerResult.Message);
                return;
            }
            IsComputer = printerResult.Object?.Computer != null;
            if (printerResult.Object?.Computer != null)
                printerResult.Object.Computer = Computers.First(x => x.Id == printerResult.Object.Computer.Id);
            Printer = printerResult.Object;
        }
    }

    public async Task SavePrinter() {
        if (!ValidationModel(Printer))
            return;
        if (!IsComputer)
            Printer.Computer = null;
        if (!Printer.IsBroken)
            Printer.BreakdownDescription = string.Empty;
        var printerResult = _windowType switch {
            WindowType.Add => await _printersService.Add(Printer),
            _ => await _printersService.Update(Printer)
        };
        if (!printerResult.Success) {
            await DialogService.ShowErrorMessageBox(printerResult.Message);
            return;
        }
        _closedWindow();
    }

    public async Task LoadComputers() {
        Computers = await _computerService.Get();
    }

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Collections

    public static IEnumerable<PrinterType> PrinterTypes => Enum.GetValues<PrinterType>();

    [Reactive]
    public partial IReadOnlyCollection<Computer>? Computers { get; private set; }

    #endregion

    #region Ctors

    public PrinterWindowViewModel(PrinterService printersService, ComputerService computerService) {
        #region Service initialization

        _printersService = printersService;

        _computerService = computerService;

        #endregion
    }
    public PrinterWindowViewModel() { }

    #endregion
}