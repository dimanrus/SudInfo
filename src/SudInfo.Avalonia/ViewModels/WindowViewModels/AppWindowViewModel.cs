namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class AppWindowViewModel : BaseViewModel
{
    #region Ctors

    public AppWindowViewModel(ComputerService computerService, AppService appService) {
        SelectionComputerChanged = ReactiveCommand.Create((int id) => {
            var computerExist = AppEntity.Computers.Any(x => x.Id == id);
            if (computerExist) {
                AppEntity.Computers.Remove(AppEntity.Computers.Single(x => x.Id == id));
                return;
            }
            AppEntity.Computers.Add(Computers!.Single(x => x.Id == id));
        });

        #region Services initialization

        _computerService = computerService;
        _appService = appService;

        #endregion
    }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Computer>? Computers { get; set; }

    #endregion

    #region Commands

    public ReactiveCommand<int, Unit> SelectionComputerChanged { get; init; }

    #endregion

    #region Services

    private readonly ComputerService _computerService;

    private readonly AppService _appService;

    #endregion

    #region Properties

    [Reactive]
    public partial AppEntity AppEntity { get; set; } = new();

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить приложение";

    [Reactive]
    public partial bool ButtonIsVisible { get; private set; }

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Public methods

    public async Task SaveApp() {
        if (!ValidationModel(AppEntity))
            return;
        var result = _windowType switch {
            WindowType.Add => await _appService.Add(AppEntity),
            _ => await _appService.Update(AppEntity)
        };
        if (!result.Success) {
            await DialogService.ShowErrorMessageBox(result.Message);
            return;
        }
        _closedWindow();
    }

    public async Task Initialization(WindowType windowType, Action closeWindow, int? id = null) {
        _windowType = windowType;
        _closedWindow = closeWindow;
        if (windowType != WindowType.View) {
            ButtonIsVisible = true;
        }
        if (id != null) {
            if (windowType != WindowType.View) {
                ButtonIsVisible = true;
                SaveButtonText = "Сохранить приложение";
            }

            var result = await _appService.Get(id.GetValueOrDefault());
            if (!result.Success) {
                await DialogService.ShowErrorMessageBox(result.Message);
                return;
            }
            AppEntity = result.Object;
        }
        Computers = await _computerService.Get();
        if (windowType == WindowType.Edit) {
            foreach (var computer in Computers) {
                computer.IsSelected = AppEntity.Computers.Any(x => x.Id == computer.Id);
            }
        }
    }

    #endregion
}