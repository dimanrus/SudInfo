namespace SudInfo.Avalonia.Views.Pages;

public partial class RutokensPage : ReactiveUserControl<RutokensPageViewModel>
{
    public RutokensPage() {
        this.WhenActivated(static disposables => { });
        InitializeComponent();
    }
}