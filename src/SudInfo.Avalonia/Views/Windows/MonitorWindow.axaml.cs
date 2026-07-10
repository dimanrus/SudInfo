namespace SudInfo.Avalonia.Views.Windows;

public partial class MonitorWindow : Window
{
    #region Constructors

    public MonitorWindow() {
        InitializeComponent();
    }
    public MonitorWindow(WindowType windowType, int? computerId = null) {
        var monitorWindowViewModel = ServiceCollectionExtension.ServiceProvider.GetService<MonitorWindowViewModel>();
        DataContext = monitorWindowViewModel;
        InitializeComponent();
        monitorWindowViewModel.Initialization(windowType, Close, computerId);
    }

    #endregion
}