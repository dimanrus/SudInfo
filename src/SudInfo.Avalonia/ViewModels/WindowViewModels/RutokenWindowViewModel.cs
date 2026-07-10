namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class RutokenWindowViewModel : BaseViewModel
{
    #region Constructors

    public RutokenWindowViewModel(
        RutokenService rutokenSerrvice,
        UserService usersService) {
        #region Service Initialization

        _rutokenService = rutokenSerrvice;

        _usersService = usersService;

        #endregion
    }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<User>? Users { get; set; }

    #endregion

    #region Services

    private readonly RutokenService _rutokenService;

    private readonly UserService _usersService;

    #endregion

    #region Properties

    [Reactive]
    public partial Rutoken Rutoken { get; set; } = new();

    [Reactive]
    public partial bool IsUser { get; set; }

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить рутокен";

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Public Methods

    public async Task SaveRutoken() {
        if (!IsUser)
            Rutoken.User = null;
        var rutokenResult = _windowType switch {
            WindowType.Add => await _rutokenService.Add(Rutoken),
            _ => await _rutokenService.Update(Rutoken)
        };
        if (!rutokenResult.Success) {
            await DialogService.ShowErrorMessageBox(rutokenResult.Message);
            return;
        }
        _closedWindow();
    }

    public async void Initialization(WindowType windowType, Action close, int? id = null) {
        _windowType = windowType;
        _closedWindow = close;

        if (id != null) {
            SaveButtonText = "Сохранить рутокен";
            var rutokenResult = await _rutokenService.Get(id.GetValueOrDefault());
            if (!rutokenResult.Success) {
                await DialogService.ShowErrorMessageBox(rutokenResult.Message);
                return;
            }
            IsUser = rutokenResult.Object?.User != null;
            if (rutokenResult.Object?.User != null)
                rutokenResult.Object.User = Users.First(x => x.Id == rutokenResult.Object.User.Id);
            Rutoken = rutokenResult.Object;
        }
    }
    public async Task LoadUsers() {
        Users = await _usersService.Get();
    }

    #endregion
}