namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class WorkplacesPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public WorkplacesPageViewModel(NavigationService navigationService, UserService userService) {
        #region Services initialization

        _navigationService = navigationService;
        _userService = userService;

        #endregion

        SearchBoxKeyUpCommand = ReactiveCommand.Create((KeyEventArgs keyEventArgs) => {
            if (string.IsNullOrWhiteSpace(SearchText)) {
                Users = UsersFromDatabase;
                return;
            }
            if (keyEventArgs.Key != Key.Enter || UsersFromDatabase == null)
                return;
            var searchTextLower = SearchText.ToLower(CultureInfo.CurrentCulture);
            Users = new List<User>(UsersFromDatabase.Where(x => x.FIO.Contains(searchTextLower, StringComparison.CurrentCultureIgnoreCase) ||
                                                                x.Computers.Any(c =>
                                                                                    c.Name!.Contains(searchTextLower, StringComparison.CurrentCultureIgnoreCase) ||
                                                                                    c.InventarNumber!.Contains(searchTextLower) ||
                                                                                    c.SerialNumber!.Contains(searchTextLower) ||
                                                                                    c.Monitors?.Any(m =>
                                                                                                        m.Name!.Contains(searchTextLower, StringComparison.CurrentCultureIgnoreCase) ||
                                                                                                        m.InventarNumber!.Contains(searchTextLower) ||
                                                                                                        m.SerialNumber!.Contains(searchTextLower)
                                                                                                   ) == true ||
                                                                                    c.Printers?.Any(p =>
                                                                                                        p.Name!.Contains(searchTextLower, StringComparison.CurrentCultureIgnoreCase) ||
                                                                                                        p.InventarNumber!.Contains(searchTextLower) ||
                                                                                                        p.SerialNumber!.Contains(searchTextLower)
                                                                                                   ) == true)));
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

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial int SelectedIndex { get; set; } = -1;

    public User? SelectedUser { get; set; }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<User>? Users { get; set; }

    public IReadOnlyCollection<User>? UsersFromDatabase { get; set; }

    #endregion

    #region Public methods

    public void CloseRowDetail() {
        SelectedIndex = -1;
    }

    public async Task OpenViewComputerWindow(object id) {
        await _navigationService.ShowComputerWindowDialog(WindowType.View, computerId: (int)id);
    }

    public async Task OpenViewPrinterWindow(object id) {
        await _navigationService.ShowPrinterWindowDialog(WindowType.View, printerId: (int)id);
    }

    public async Task OpenViewMonitorWindow(object id) {
        await _navigationService.ShowMonitorWindowDialog(WindowType.View, monitorId: (int)id);
    }

    public async Task OpenViewPeripheryWindow(object id) {
        await _navigationService.ShowPeripheryWindowDialog(WindowType.View, peripheryId: (int)id);
    }

    public async Task LoadWorkplaces() {
        Users = await _userService.Get();
        UsersFromDatabase = Users;
    }

    #endregion
}