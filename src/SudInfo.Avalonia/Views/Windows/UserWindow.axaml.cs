namespace SudInfo.Avalonia.Views.Windows;

public partial class UserWindow : ReactiveWindow<UserWindowViewModel>
{
    #region Constructors

    public UserWindow() { }
    public UserWindow(WindowType windowType, int? printerId = null) {
        var userWindowViewModel = ServiceCollectionExtension.ServiceProvider.GetService<UserWindowViewModel>();
        DataContext = userWindowViewModel;
        InitializeComponent();
        userWindowViewModel?.Initialization(windowType, Close, printerId);
    }

    #endregion
}