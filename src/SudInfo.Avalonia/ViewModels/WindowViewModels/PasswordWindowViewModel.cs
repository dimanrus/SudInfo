namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class PasswordWindowViewModel : BaseViewModel
{
    #region Services

    private readonly PasswordService _passwordService;

    #endregion

    #region Constructors

    public PasswordWindowViewModel(PasswordService passwordService) {
        #region Service Set

        _passwordService = passwordService;

        #endregion
    }

    #endregion

    #region Properties

    [Reactive]
    public partial PasswordEntity Password { get; set; } = new();

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить пароль";

    [Reactive]
    public partial bool ButtonIsVisible { get; private set; } = false;

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Public Methods

    public async Task SavePassword() {
        if (!ValidationModel(Password))
            return;
        var result = _windowType switch {
            WindowType.Add => await _passwordService.Add(Password),
            _ => await _passwordService.Update(Password)
        };
        if (!result.Success) {
            await DialogService.ShowErrorMessageBox(result.Message);
            return;
        }
        _closedWindow();
    }

    public async void Initialization(WindowType windowType, Action close, int? id = null) {
        _windowType = windowType;
        _closedWindow = close;

        if (windowType != WindowType.View) {
            ButtonIsVisible = true;
        }
        if (id != null) {
            if (windowType != WindowType.View) {
                ButtonIsVisible = true;
                SaveButtonText = "Сохранить пароль";
            }
            var result = await _passwordService.Get(id.GetValueOrDefault());
            if (!result.Success) {
                await DialogService.ShowErrorMessageBox(result.Message);
                return;
            }
            Password = result.Object;
        }
    }

    #endregion
}