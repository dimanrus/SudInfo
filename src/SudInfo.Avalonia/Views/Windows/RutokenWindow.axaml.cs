namespace SudInfo.Avalonia.Views.Windows;

public partial class RutokenWindow : ReactiveWindow<RutokenWindowViewModel>
{
    #region Constructors

    public RutokenWindow() { }
    public RutokenWindow(WindowType windowType, int? rutokenId = null) {
        var viewModel = ServiceCollectionExtension.ServiceProvider.GetService<RutokenWindowViewModel>()!;
        DataContext = viewModel;
        InitializeComponent();
        viewModel.Initialization(windowType, Close, rutokenId);
    }

    #endregion
}