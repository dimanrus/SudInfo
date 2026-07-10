namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class ComputerWindowViewModel : BaseViewModel
{
    #region Ctors

    public ComputerWindowViewModel(ComputerService computerService, UserService userService) {
        #region Service Initialization

        _computerService = computerService;
        _userService = userService;

        #endregion
    }

    #endregion

    #region Services

    private readonly ComputerService _computerService;

    private readonly UserService _userService;

    #endregion

    #region Collections

    public static IEnumerable<TypeROM> RomTypes => Enum.GetValues<TypeROM>();

    public static IEnumerable<OS> OsesList => Enum.GetValues<OS>();

    [Reactive]
    public partial IReadOnlyCollection<User>? Users { get; set; }

    #endregion

    #region Properties

    [Reactive]
    public partial Computer Computer { get; set; } = new();

    [Reactive]
    public partial bool IsUser { get; set; }

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить компьютер";

    [Reactive]
    public partial bool ButtonIsVisible { get; private set; }

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Public Methods

    public async Task SaveComputer() {
        if (!ValidationModel(Computer))
            return;
        if (!IsUser)
            Computer.User = null;
        if (!Computer.IsBroken)
            Computer.BreakdownDescription = string.Empty;
        var computerResult = _windowType switch {
            WindowType.Add => await _computerService.Add(Computer),
            _ => await _computerService.Update(Computer)
        };
        if (!computerResult.Success) {
            await DialogService.ShowErrorMessageBox(computerResult.Message);
            return;
        }
        _closedWindow();
    }

    public async void Initialization(WindowType windowType, Action close, int? id = null) {
        Users = await _userService.Get();

        _windowType = windowType;
        _closedWindow = close;

        if (windowType != WindowType.View) {
            ButtonIsVisible = true;
        }
        if (id == null)
            return;
        if (windowType != WindowType.View) {
            ButtonIsVisible = true;
            SaveButtonText = "Сохранить компьютер";
        }
        var computerResult = await _computerService.Get(id.GetValueOrDefault());
        if (!computerResult.Success) {
            await DialogService.ShowErrorMessageBox(computerResult.Message);
            return;
        }
        IsUser = computerResult.Object?.User != null;
        if (computerResult.Object?.User != null)
            computerResult.Object.User = Users.First(x => x.Id == computerResult.Object.User.Id);
        Computer = computerResult.Object;
    }

    #endregion
}