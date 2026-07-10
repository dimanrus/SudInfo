namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public partial class ServerRackWindowViewModel : BaseViewModel
{
    #region Services

    private readonly ServerRackService _serverRackService;

    #endregion

    #region Constructors

    public ServerRackWindowViewModel(ServerRackService serverRackService) {
        #region Service Set

        _serverRackService = serverRackService;

        #endregion
    }

    #endregion

    #region Private Fields

    private WindowType _windowType;

    private Action _closedWindow;

    #endregion

    #region Properties

    [Reactive]
    public partial ServerRack ServerRack { get; set; } = new();

    [Reactive]
    public partial string SaveButtonText { get; private set; } = "Добавить серверную стойку";

    #endregion

    #region Public Methods

    public async void Initialization(WindowType windowType, Action close, int? id = null) {
        _windowType = windowType;
        _closedWindow = close;

        if (id != null) {
            SaveButtonText = "Сохранить серверную стойку";
            var server = await _serverRackService.Get(id.GetValueOrDefault());
            if (!server.Success) {
                await DialogService.ShowErrorMessageBox(server.Message);
                return;
            }
            ServerRack = server.Object;
        }
    }

    public async Task SaveServerRack() {
        if (_windowType == WindowType.Add) {
            var result = await _serverRackService.GetNumberServerRacks();
            ServerRack.Position = ++result;
        }
        var serverRackResult = _windowType switch {
            WindowType.Add => await _serverRackService.Add(ServerRack),
            _ => await _serverRackService.Update(ServerRack)
        };
        if (!serverRackResult.Success) {
            await DialogService.ShowErrorMessageBox(serverRackResult.Message);
            return;
        }
        _closedWindow();
    }

    #endregion
}