namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class CartridgesPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public CartridgesPageViewModel(
        CartridgeService cartridgeService,
        NavigationService navigationService) {
        #region Services Initialization

        _cartridgeService = cartridgeService;

        _navigationService = navigationService;

        #endregion

        EventHandlerClosedWindowDialog += async (s, e) => { await LoadCartridges(); };
    }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Cartridge>? Cartridges { get; set; }

    #endregion

    #region Services

    private readonly CartridgeService _cartridgeService;

    private readonly NavigationService _navigationService;

    #endregion

    #region Properties

    public Cartridge? SelectedCartridge { get; set; }

    [Reactive]
    public partial Cartridge NewCartridge { get; set; } = new();

    #endregion

    #region Public methods

    public async Task CreateExcelTable() {
        if (Cartridges == null || Cartridges.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Cartridges);
    }

    public async Task OpenEditCartridgeWindow() {
        if (SelectedCartridge == null)
            return;
        await _navigationService.ShowCartridgeWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, SelectedCartridge.Id);
    }

    public async Task OpenAddCartridgeWindow() {
        await _navigationService.ShowCartridgeWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task LoadCartridges() {
        Cartridges = await _cartridgeService.Get();
    }

    public async Task RemoveCartridge() {
        if (SelectedCartridge == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить картридж?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeCartridgesResult = await _cartridgeService.Remove(SelectedCartridge.Id);
        if (!removeCartridgesResult.Success) {
            await DialogService.ShowErrorMessageBox(removeCartridgesResult.Message);
            return;
        }
        await LoadCartridges();
    }

    #endregion
}