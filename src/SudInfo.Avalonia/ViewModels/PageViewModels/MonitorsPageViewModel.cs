namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class MonitorsPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public MonitorsPageViewModel(
        NavigationService navigationService,
        MonitorService monitorsService) {
        #region Serives Initialization

        _monitorService = monitorsService;
        _navigationService = navigationService;

        #endregion

        EventHandlerClosedWindowDialog = async (s, e) => await LoadMonitors();

        SearchBoxKeyUpCommand = ReactiveCommand.Create((KeyEventArgs keyEventArgs) => {
            if (string.IsNullOrEmpty(SearchText)) {
                Monitors = MonitorsFromDataBase;
                return;
            }
            if (keyEventArgs.Key != Key.Enter || MonitorsFromDataBase == null)
                return;
            Monitors = [
                .. MonitorsFromDataBase.Where(x => x.Name!.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                                   x.InventarNumber?.Contains(SearchText) == true ||
                                                   x.SerialNumber!.Contains(SearchText) ||
                                                   x.Computer != null &&
                                                   x.Computer.User?.FIO.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) == true)
            ];
        });
    }

    #endregion

    #region Commands

    public ReactiveCommand<KeyEventArgs, Unit> SearchBoxKeyUpCommand { get; set; }

    #endregion

    #region Services

    private readonly MonitorService _monitorService;

    private readonly NavigationService _navigationService;

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Monitor>? Monitors { get; set; }

    private IReadOnlyCollection<Monitor>? MonitorsFromDataBase { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial Monitor? SelectedMonitor { get; set; }

    #endregion

    #region Public Methods

    public async Task CreateExcelTable() {
        if (Monitors == null || Monitors.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Monitors);
    }

    public async Task OpenAddMonitorWindow() {
        await _navigationService.ShowMonitorWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task OpenEditMonitorWindow() {
        if (SelectedMonitor == null)
            return;
        await _navigationService.ShowMonitorWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, SelectedMonitor.Id);
    }

    public async Task RemoveMonitor() {
        if (SelectedMonitor == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить монитор?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeMonitorResult = await _monitorService.Remove(SelectedMonitor.Id);
        if (!removeMonitorResult.Success) {
            await DialogService.ShowErrorMessageBox(removeMonitorResult.Message);
            return;
        }
        await LoadMonitors();
    }

    public async Task LoadMonitors() {
        Monitors = await _monitorService.Get();
        MonitorsFromDataBase = Monitors;
    }

    #endregion
}