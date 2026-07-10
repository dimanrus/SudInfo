namespace SudInfo.Avalonia.Views.Windows;

public partial class PhoneWindow : ReactiveWindow<PhoneWindowViewModel>
{
    #region Ctors

    public PhoneWindow(WindowType windowType, int? computerId = null) {
        InitializeComponent();
        var viewModel = ServiceCollectionExtension.ServiceProvider.GetService<PhoneWindowViewModel>();
        DataContext = viewModel;
        viewModel.Initialization(windowType, Close, computerId);
    }

    #endregion
}