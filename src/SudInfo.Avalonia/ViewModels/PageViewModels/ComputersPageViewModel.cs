namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class ComputersPageViewModel : BaseRoutableViewModel
{
    #region Private Variables

    private readonly EventHandler _eventHandlerClosedWindowDialog;

    #endregion

    #region Ctors

    public ComputersPageViewModel(NavigationService navigationService, ComputerService computerService) {
        #region Services Initialization

        _navigationService = navigationService;
        _computerService = computerService;

        #endregion

        _eventHandlerClosedWindowDialog = async (s, e) => await LoadComputers();

        SearchBoxKeyUpCommand = ReactiveCommand.Create((KeyEventArgs keyEventArgs) => {
            if (string.IsNullOrEmpty(SearchText)) {
                Computers = ComputersFromDataBase;
                return;
            }
            if (keyEventArgs.Key != Key.Enter || ComputersFromDataBase == null)
                return;
            Computers = [
                .. ComputersFromDataBase.Where(x => x.Name!.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
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

    private readonly ComputerService _computerService;

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Computer>? Computers { get; set; }

    private IReadOnlyCollection<Computer>? ComputersFromDataBase { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial string SearchText { get; set; } = string.Empty;

    [Reactive]
    public partial Computer? SelectedComputer { get; set; }

    #endregion

    #region Public Methods

    public async Task OpenAddComputerWindow() {
        await _navigationService.ShowComputerWindowDialog(WindowType.Add, _eventHandlerClosedWindowDialog);
    }

    public async Task OpenEditComputerWindow() {
        if (SelectedComputer == null)
            return;
        await _navigationService.ShowComputerWindowDialog(WindowType.Edit, _eventHandlerClosedWindowDialog, SelectedComputer.Id);
    }

    public async Task RemoveComputer() {
        if (SelectedComputer == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить компьютер?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeComputerResult = await _computerService.Remove(SelectedComputer.Id);
        if (!removeComputerResult.Success) {
            await DialogService.ShowErrorMessageBox(removeComputerResult.Message);
            return;
        }

        await LoadComputers();
    }

    public async Task CreateExcelTable() {
        if (Computers == null || Computers.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Computers);
    }

    public async Task LoadComputers() {
        SearchText = string.Empty;
        Computers = await _computerService.Get();
        ComputersFromDataBase = Computers;
    }

    #endregion
}