namespace SudInfo.Avalonia.Views.Pages;

public partial class MonitorsPage : ReactiveUserControl<MonitorsPageViewModel>
{
    public MonitorsPage() {
        this.WhenActivated(static disposables => { });
        InitializeComponent();
    }
}