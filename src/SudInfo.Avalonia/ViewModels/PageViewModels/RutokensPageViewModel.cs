namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class RutokensPageViewModel : BaseRoutableViewModel
{
    #region Private Variables

    private readonly EventHandler _eventHandlerClosedWindowDialog;

    #endregion

    #region Ctors

    public RutokensPageViewModel(NavigationService navigationService, RutokenService rutokenService) {
        #region Serives Initialization

        _navigationService = navigationService;
        _rutokenService = rutokenService;

        #endregion

        _eventHandlerClosedWindowDialog = async (s, e) => await LoadRutokens();
    }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Rutoken>? Rutokens { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial Rutoken? SelectedRutoken { get; set; }

    #endregion

    #region Services

    private readonly NavigationService _navigationService;

    private readonly RutokenService _rutokenService;

    #endregion

    #region Public Methods

    public async Task CreateExcelTable() {
        if (Rutokens == null || Rutokens.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Rutokens);
    }

    public async Task OpenAddRutokenWindow() {
        await _navigationService.ShowRutokenWindowDialog(WindowType.Add, _eventHandlerClosedWindowDialog);
    }

    public async Task OpenEditRutokenWindow() {
        if (SelectedRutoken == null)
            return;
        await _navigationService.ShowRutokenWindowDialog(WindowType.Edit, _eventHandlerClosedWindowDialog, SelectedRutoken.Id);
    }

    public async Task RemoveRutoken() {
        if (SelectedRutoken == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить рутокен?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeRutokenResult = await _rutokenService.Remove(SelectedRutoken.Id);
        if (!removeRutokenResult.Success) {
            await DialogService.ShowErrorMessageBox(removeRutokenResult.Message);
            return;
        }
        await LoadRutokens();
    }

    public async Task LoadRutokens() {
        Rutokens = await _rutokenService.Get();
    }

    #endregion
}