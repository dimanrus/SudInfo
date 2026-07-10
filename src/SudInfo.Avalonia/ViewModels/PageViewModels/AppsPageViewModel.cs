namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class AppsPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public AppsPageViewModel(
        AppService appService,
        NavigationService navigationService) {
        #region Services initialization

        _appService = appService;

        _navigationService = navigationService;

        #endregion

        EventHandlerClosedWindowDialog = async (s, e) => await LoadApps();
    }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<AppEntity>? Apps { get; set; }

    #endregion

    #region Services

    private readonly AppService _appService;

    private readonly NavigationService _navigationService;

    #endregion

    #region Properties

    [Reactive]
    public partial int SelectedIndex { get; set; } = -1;

    public AppEntity? SelectedApp { get; set; }

    #endregion

    #region Public Methods

    public void CloseRowDetail() {
        SelectedIndex = -1;
    }

    public async Task LoadApps() {
        Apps = await _appService.Get();
    }

    public async Task OpenAddAppWindow() {
        await _navigationService.ShowAppWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task OpenEditAppWindow() {
        if (SelectedApp == null)
            return;
        await _navigationService.ShowAppWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, SelectedApp.Id);
    }

    public async Task RemoveApp() {
        if (SelectedApp == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить приложение?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeComputerResult = await _appService.Remove(SelectedApp.Id);
        if (!removeComputerResult.Success) {
            await DialogService.ShowErrorMessageBox(removeComputerResult.Message);
            return;
        }
        await LoadApps();
    }

    public async Task CreateExcelTable() {
        if (Apps == null || Apps.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Apps);
    }

    #endregion
}