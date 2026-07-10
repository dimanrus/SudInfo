namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class PhonesPageViewModel : BaseRoutableViewModel
{
    #region Private Variables

    private readonly EventHandler _eventHandlerClosedWindowDialog;

    #endregion

    #region Ctors

    public PhonesPageViewModel(NavigationService navigationService, PhoneService phoneService) {
        #region Services Initialization

        _navigationService = navigationService;
        _phoneService = phoneService;

        #endregion

        _eventHandlerClosedWindowDialog = async (s, e) => await LoadPhones();

        SearchBoxKeyUpCommand = ReactiveCommand.Create((KeyEventArgs keyEventArgs) => {
            if (string.IsNullOrEmpty(SearchText)) {
                Phones = PhonesFromDataBase;
                return;
            }
            if (keyEventArgs.Key != Key.Enter || PhonesFromDataBase == null)
                return;
            Phones = [
                .. PhonesFromDataBase.Where(x => x.Name!.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                                 x.InventarNumber?.Contains(SearchText) == true ||
                                                 x.SerialNumber!.Contains(SearchText) ||
                                                 x.User?.FIO.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) == true)
            ];
        });
    }

    #endregion

    #region Commands

    public ReactiveCommand<KeyEventArgs, Unit> SearchBoxKeyUpCommand { get; set; }

    #endregion

    #region Services

    private readonly NavigationService _navigationService;

    private readonly PhoneService _phoneService;

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Phone>? Phones { get; set; }

    private IReadOnlyCollection<Phone>? PhonesFromDataBase { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial Phone? SelectedPhone { get; set; }

    #endregion

    #region Public Methods

    public async Task OpenAddPhoneWindow() {
        await _navigationService.ShowPhoneWindowDialog(WindowType.Add, _eventHandlerClosedWindowDialog);
    }

    public async Task OpenEditPhoneWindow() {
        if (SelectedPhone == null)
            return;
        await _navigationService.ShowPhoneWindowDialog(WindowType.Edit, _eventHandlerClosedWindowDialog, SelectedPhone.Id);
    }

    public async Task RemovePhone() {
        if (SelectedPhone == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить телефон?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeComputerResult = await _phoneService.Remove(SelectedPhone.Id);
        if (!removeComputerResult.Success) {
            await DialogService.ShowErrorMessageBox(removeComputerResult.Message);
            return;
        }

        await LoadPhones();
    }

    public async Task CreateExcelTable() {
        if (Phones == null || Phones.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Phones);
    }

    public async Task LoadPhones() {
        SearchText = string.Empty;
        Phones = await _phoneService.Get();
        PhonesFromDataBase = Phones;
    }

    #endregion
}