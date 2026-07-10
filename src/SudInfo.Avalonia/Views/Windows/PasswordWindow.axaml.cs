namespace SudInfo.Avalonia.Views.Windows;

public partial class PasswordWindow : ReactiveWindow<PasswordWindowViewModel>
{
    public PasswordWindow() {
        InitializeComponent();
    }
    public PasswordWindow(WindowType windowType, int? id = null) {
        var viewModel = ServiceCollectionExtension.ServiceProvider.GetService<PasswordWindowViewModel>();
        DataContext = viewModel;
        InitializeComponent();
        viewModel.Initialization(windowType, Close, id);
    }
}