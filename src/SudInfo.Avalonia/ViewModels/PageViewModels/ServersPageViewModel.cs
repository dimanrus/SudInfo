namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class ServersPageViewModel : BaseRoutableViewModel
{
    #region Ctors

    public ServersPageViewModel(NavigationService navigationService, ServerService serverService, ServerRackService serverRackService) {
        #region Services Initialization

        _navigationService = navigationService;
        _serverService = serverService;
        _serverRackService = serverRackService;

        #endregion

        EventHandlerClosedWindowDialog = async (s, e) => await LoadServerRacks();

        #region Commands Initialization

        OpenAddServerWindowCommand = ReactiveCommand.Create<ServerRack>(async serverRack => { await _navigationService.ShowServerWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog, serverRack: serverRack); });

        RemoveServerCommand = ReactiveCommand.Create<int>(async id => {
            var result = await _serverService.Remove(id);
            if (!result.Success) {
                await DialogService.ShowErrorMessageBox(result.Message);
                return;
            }
            await LoadServerRacks();
        });

        RemoveServerRackCommand = ReactiveCommand.Create<int>(async id => {
            var result = await _serverRackService.Remove(id);
            if (!result.Success) {
                await DialogService.ShowErrorMessageBox(result.Message);
                return;
            }
            await LoadServerRacks();
        });

        EditServerCommand = ReactiveCommand.Create<int>(async id => { await _navigationService.ShowServerWindowDialog(WindowType.Edit, EventHandlerClosedWindowDialog, id); });

        ViewServerCommand = ReactiveCommand.Create<int>(async id => { await _navigationService.ShowServerWindowDialog(WindowType.View, id: id); });

        UpServerPositionInServerRack = ReactiveCommand.Create<int>(async id => {
            var result = await _serverService.UpServerPositionInServerRack(id);
            if (!result.Success)
                await DialogService.ShowErrorMessageBox(result.Message);
            await LoadServerRacks();
        });

        DownServerPositionInServerRack = ReactiveCommand.Create<int>(async id => {
            var result = await _serverService.DownServerPositionInServerRack(id);
            if (!result.Success)
                await DialogService.ShowErrorMessageBox(result.Message);
            await LoadServerRacks();
        });

        #endregion
    }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<ServerRack>? ServerRacks { get; set; }

    #endregion

    #region Services

    private readonly NavigationService _navigationService;

    private readonly ServerRackService _serverRackService;

    private readonly ServerService _serverService;

    #endregion

    #region Public Methods

    public async Task OpenAddServerRackWindow() {
        await _navigationService.ShowServerRackWindowDialog(WindowType.Add, EventHandlerClosedWindowDialog);
    }

    public async Task LoadServerRacks() {
        var serverRacksResult = await _serverRackService.Get();
        if (!serverRacksResult.Success) {
            await DialogService.ShowErrorMessageBox(serverRacksResult.Message);
            return;
        }
        ServerRacks = serverRacksResult.Object;
    }

    #endregion

    #region Commands

    public ReactiveCommand<ServerRack, Unit> OpenAddServerWindowCommand { get; init; }

    public ReactiveCommand<int, Unit> RemoveServerCommand { get; init; }

    public ReactiveCommand<int, Unit> RemoveServerRackCommand { get; init; }

    public ReactiveCommand<int, Unit> ViewServerCommand { get; init; }

    public ReactiveCommand<int, Unit> EditServerCommand { get; init; }

    public ReactiveCommand<int, Unit> UpServerPositionInServerRack { get; init; }

    public ReactiveCommand<int, Unit> DownServerPositionInServerRack { get; init; }

    #endregion
}