namespace SudInfo.Avalonia.Views.Windows;

public partial class ComputerWindow : ReactiveWindow<ComputerWindowViewModel>
{
    #region Ctors

    public ComputerWindow() { }

    public ComputerWindow(WindowType windowType, int? computerId = null) {
        InitializeComponent();
        var computerWindowViewModel = ServiceCollectionExtension.ServiceProvider.GetService<ComputerWindowViewModel>();
        DataContext = computerWindowViewModel;
        computerWindowViewModel.Initialization(windowType, Close, computerId);
    }

    #endregion
}