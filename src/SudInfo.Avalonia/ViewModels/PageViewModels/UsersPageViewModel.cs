namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class UsersPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public UsersPageViewModel(NavigationService navigationService, UserService userService) {
        #region Services Initialization

        _navigationService = navigationService;
        _userService = userService;

        #endregion

        EventHandlerClosedWindowDialog = async (s, e) => await LoadUsers();

        SearchBoxKeyUpCommand = ReactiveCommand.Create((KeyEventArgs keyEventArgs) => {
            if (string.IsNullOrEmpty(SearchText)) {
                Users = UsersFromDataBase;
                return;
            }
            if (keyEventArgs.Key != Key.Enter || UsersFromDataBase == null)
                return;
            Users = [.. UsersFromDataBase.Where(x => x.FIO!.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase))];
        });
    }

    #endregion

    #region Commands

    public ReactiveCommand<KeyEventArgs, Unit> SearchBoxKeyUpCommand { get; set; }

    #endregion

    #region Services

    private readonly NavigationService _navigationService;

    private readonly UserService _userService;

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<User>? Users { get; set; }

    private IReadOnlyCollection<User>? UsersFromDataBase { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial User? SelectedUser { get; set; }

    #endregion

    #region Public Methods

    public async Task CreateExcelTable() {
        if (Users == null || Users.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Users);
    }

    public async Task OpenAddUserWindow() {
        await _navigationService.ShowUserWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task OpenEditUserWindow() {
        if (SelectedUser == null)
            return;
        await _navigationService.ShowUserWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, SelectedUser.Id);
    }

    public async Task RemoveUser() {
        if (SelectedUser == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить пользователя?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeUserResult = await _userService.Remove(SelectedUser.Id);
        if (!removeUserResult.Success) {
            await DialogService.ShowErrorMessageBox(removeUserResult.Message);
            return;
        }
        await LoadUsers();
    }

    public async Task LoadUsers() {
        Users = await _userService.Get();
        UsersFromDataBase = Users;
    }

    #endregion
}