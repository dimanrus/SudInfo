namespace SudInfo.Avalonia.Views.Windows;

public partial class ContactWindow : ReactiveWindow<ContactWindowViewModel>
{
    public ContactWindow() {
        InitializeComponent();
    }
    public ContactWindow(WindowType windowType, int? id = null) {
        var viewModel = ServiceCollectionExtension.ServiceProvider.GetService<ContactWindowViewModel>();
        DataContext = viewModel;
        InitializeComponent();
        viewModel.Initialization(windowType, Close, id);
    }
}