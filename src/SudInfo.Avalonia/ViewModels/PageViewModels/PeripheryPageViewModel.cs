namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class PeripheryPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public PeripheryPageViewModel(
        PeripheryService peripheryService,
        NavigationService navigationService) {
        #region Services Initialization

        _peripheryService = peripheryService;

        _navigationService = navigationService;

        #endregion

        EventHandlerClosedWindowDialog = async (s, e) => await LoadPeripheries();
    }

    #endregion

    #region Services

    private readonly PeripheryService _peripheryService;

    private readonly NavigationService _navigationService;

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Periphery>? Peripheries { get; set; }

    private IReadOnlyCollection<Periphery>? PeripheriesFromDatabase { get; set; }

    public static IEnumerable<PeripheryType> PeripheryTypes => Enum.GetValues<PeripheryType>();

    #endregion

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial Periphery? SelectedPeriphery { get; set; }

    [Reactive]
    public partial PeripheryType SelectedPeripheryType { get; set; } = Enum.GetValues<PeripheryType>().First();

    [Reactive]
    public partial bool IsPeripheryTypeFilter { get; set; }

    #endregion

    #region Public Methods

    public async Task CreateExcelTable() {
        if (Peripheries == null || Peripheries.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Peripheries);
    }

    public async Task OpenAddPeripheryWindow() {
        await _navigationService.ShowPeripheryWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task OpenEditPeripheryWindow() {
        if (SelectedPeriphery == null)
            return;
        await _navigationService.ShowPeripheryWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, SelectedPeriphery.Id);
    }

    public async Task RemovePeriphery() {
        if (SelectedPeriphery == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить периферию?");
        if (dialogResult == ButtonResult.No)
            return;
        var removePeripheryResult = await _peripheryService.Remove(SelectedPeriphery.Id);
        if (!removePeripheryResult.Success) {
            await DialogService.ShowErrorMessageBox(removePeripheryResult.Message);
            return;
        }
        await LoadPeripheries();
    }

    public void SearchBoxKeyUp(object? checkedChangedEventArgs = null) {
        if (checkedChangedEventArgs != null) {
            IsPeripheryTypeFilter = (bool)((CheckBox)((RoutedEventArgs)checkedChangedEventArgs).Source).IsChecked;
        }
        if (PeripheriesFromDatabase == null || Peripheries == null) {
            return;
        }
        Peripheries = [
            .. PeripheriesFromDatabase.Where(x => x.Name!.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                                  x.InventarNumber?.Contains(SearchText) == true ||
                                                  x.SerialNumber!.Contains(SearchText) ||
                                                  x.Computer != null &&
                                                  x.Computer.User?.FIO.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) == true)
        ];
        if (IsPeripheryTypeFilter) {
            Peripheries = [.. Peripheries.Where(x => IsPeripheryTypeFilter && x.Type == SelectedPeripheryType)];
        }
    }

    public void SelectionPeripheryTypeChanged(object selectionChangedEventArgs) {
        if (Peripheries == null)
            return;
        SelectedPeripheryType = (PeripheryType)((ComboBox)((SelectionChangedEventArgs)selectionChangedEventArgs).Source).SelectedItem;
        SearchBoxKeyUp();
    }

    public async Task LoadPeripheries() {
        Peripheries = await _peripheryService.Get();
        PeripheriesFromDatabase = Peripheries;
        SearchText = string.Empty;
    }

    #endregion
}