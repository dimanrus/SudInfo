namespace SudInfo.Avalonia.Views.Windows;

public partial class ServerRackWindow : ReactiveWindow<ServerRackWindowViewModel>
{
    public ServerRackWindow() {
        InitializeComponent();
    }
    public ServerRackWindow(WindowType windowType, int? id = null) {
        var viewModel = ServiceCollectionExtension.ServiceProvider.GetService<ServerRackWindowViewModel>()!;
        DataContext = viewModel;
        InitializeComponent();
        viewModel.Initialization(windowType, Close, id);
    }
}