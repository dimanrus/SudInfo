namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class PasswordsPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public PasswordsPageViewModel(NavigationService navigationService, PasswordService passwordService) {
        #region Serives Initialization

        _navigationService = navigationService;
        _passwordService = passwordService;

        #endregion

        EventHandlerClosedWindowDialog = async (s, e) => await LoadPasswords();

        SearchBoxKeyUpCommand = ReactiveCommand.Create((KeyEventArgs keyEventArgs) => {
            if (string.IsNullOrEmpty(SearchText)) {
                Passwords = PasswordsFromDatabase;
                return;
            }
            if (keyEventArgs.Key != Key.Enter || PasswordsFromDatabase == null)
                return;
            Passwords = [.. PasswordsFromDatabase.Where(x => x.Description!.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase))];
        });
    }

    #endregion

    #region Commands

    public ReactiveCommand<KeyEventArgs, Unit> SearchBoxKeyUpCommand { get; set; }

    #endregion

    #region Services

    private readonly NavigationService _navigationService;

    private readonly PasswordService _passwordService;

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<PasswordEntity>? Passwords { get; set; }

    private IReadOnlyCollection<PasswordEntity>? PasswordsFromDatabase { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial PasswordEntity? SelectedPassword { get; set; }

    #endregion

    #region Public Methods

    public async Task CreateExcelTable() {
        if (Passwords == null || Passwords.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Passwords);
    }

    public async Task LoadPasswords() {
        Passwords = await _passwordService.Get();
        PasswordsFromDatabase = Passwords;
    }

    public async Task OpenAddPasswordWindow() {
        await _navigationService.ShowPasswordWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task OpenEditPasswordWindow() {
        if (SelectedPassword == null)
            return;
        await _navigationService.ShowPasswordWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, SelectedPassword.Id);
    }

    public async Task RemovePassword() {
        if (SelectedPassword == null)
            return;
        var result = await _passwordService.Remove(SelectedPassword.Id);
        if (!result.Success) {
            await DialogService.ShowErrorMessageBox(result.Message);
            return;
        }
        await LoadPasswords();
    }

    #endregion
}