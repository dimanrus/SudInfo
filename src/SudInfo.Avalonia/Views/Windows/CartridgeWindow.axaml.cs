namespace SudInfo.Avalonia.Views.Windows;

public partial class CartridgeWindow : ReactiveWindow<CartridgeWindowViewModel>
{
    public CartridgeWindow(WindowType windowType, int? id = null) {
        var viewModel = ServiceCollectionExtension.ServiceProvider.GetService<CartridgeWindowViewModel>()!;
        DataContext = viewModel;
        InitializeComponent();
        viewModel.Initialization(windowType, Close, id);
    }
}