namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class ContactWindowViewModel : BaseViewModel
{
    #region Services

    private readonly ContactService _contactService;

    #endregion

    #region Constructors

    public ContactWindowViewModel(ContactService contactService) {
        #region Service Set

        _contactService = contactService;

        #endregion
    }

    #endregion

    #region Properties

    [Reactive]
    public partial Contact Contact { get; set; } = new();

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить контакт";

    [Reactive]
    public partial bool ButtonIsVisible { get; private set; } = false;

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Public Methods

    public async Task SaveContact() {
        if (!ValidationModel(Contact))
            return;
        var result = _windowType switch {
            WindowType.Add => await _contactService.Add(Contact),
            _ => await _contactService.Update(Contact)
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
                SaveButtonText = "Сохранить контакт";
            }
            var result = await _contactService.Get(id.GetValueOrDefault());
            if (!result.Success) {
                await DialogService.ShowErrorMessageBox(result.Message);
                return;
            }
            Contact = result.Object;
        }
    }

    #endregion
}