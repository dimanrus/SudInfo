namespace SudInfo.Avalonia.Views.Windows;

public partial class AppWindow : ReactiveWindow<AppWindowViewModel>
{
    public AppWindow() {
        InitializeComponent();
    }
    public AppWindow(WindowType windowType, int? id = null) {
        var viewModel = ServiceCollectionExtension.ServiceProvider.GetService<AppWindowViewModel>();
        DataContext = viewModel;
        InitializeComponent();
        viewModel.Initialization(windowType, Close, id);
    }
}