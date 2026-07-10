namespace SudInfo.Avalonia.Views.Pages;

public partial class PasswordsPage : ReactiveUserControl<PasswordsPageViewModel>
{
    public PasswordsPage() {
        this.WhenActivated(static disposables => { });
        InitializeComponent();
    }
}