namespace SudInfo.Avalonia.Views.Windows;

public partial class PrinterWindow : ReactiveWindow<PrinterWindowViewModel>
{
    #region Constructors

    public PrinterWindow() { }
    public PrinterWindow(WindowType windowType, int? printerId = null) {
        var printerWindowViewModel = ServiceCollectionExtension.ServiceProvider.GetService<PrinterWindowViewModel>();
        DataContext = printerWindowViewModel;
        InitializeComponent();
        printerWindowViewModel.Initialization(windowType, Close, printerId);
    }

    #endregion
}