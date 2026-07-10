namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class ServerWindowViewModel : BaseViewModel
{
    #region Services

    private readonly ServerService _serverService;

    #endregion

    #region Constructors

    public ServerWindowViewModel(ServerService serverService) {
        #region Service Set

        _serverService = serverService;

        #endregion
    }

    #endregion

    #region Collections

    public static IEnumerable<ServerOperatingSystem> OperatingSystems => Enum.GetValues<ServerOperatingSystem>();

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Properties

    [Reactive]
    public partial Server Server { get; set; } = new();

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить сервер";

    [Reactive]
    public partial bool ButtonIsVisible { get; private set; } = false;

    #endregion

    #region Public Methods

    public async void Initialization(WindowType windowType, Action close, int? id = null, ServerRack? serverRack = null) {
        _windowType = windowType;
        _closedWindow = close;

        if (windowType != WindowType.View) {
            ButtonIsVisible = true;
        }
        if (serverRack != null) {
            Server.ServerRackId = serverRack.Id;
            Server.PosiitionInServerRack = serverRack.Servers.Count + 1;
        }
        if (id != null) {
            if (windowType != WindowType.View) {
                ButtonIsVisible = true;
                SaveButtonText = "Сохранить сервер";
            }
            var server = await _serverService.Get(id.GetValueOrDefault());
            if (!server.Success) {
                await DialogService.ShowErrorMessageBox(server.Message);
                return;
            }
            Server = server.Object;
        }
    }

    public async Task SaveServer() {
        if (!ValidationModel(Server))
            return;
        var serverResult = _windowType switch {
            WindowType.Add => await _serverService.Add(Server),
            _ => await _serverService.Update(Server)
        };
        if (!serverResult.Success) {
            await DialogService.ShowErrorMessageBox(serverResult.Message);
            return;
        }
        _closedWindow();
    }

    #endregion
}