namespace SudInfo.Avalonia.Views.Pages;

public partial class ComputersPage : ReactiveUserControl<ComputersPageViewModel>
{
    public ComputersPage() {
        this.WhenActivated(static disposables => { });
        InitializeComponent();
    }
}