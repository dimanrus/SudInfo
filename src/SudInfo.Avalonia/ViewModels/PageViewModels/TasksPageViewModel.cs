namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class TasksPageViewModel : BaseRoutableViewModel
{
    #region Private Variables

    private readonly EventHandler _eventHandlerClosedWindowDialog;

    #endregion

    #region Ctors

    public TasksPageViewModel(NavigationService navigationService, TaskService taskService) {
        #region Serives Initialization

        _navigationService = navigationService;
        _taskService = taskService;

        #endregion

        _eventHandlerClosedWindowDialog = async (s, e) => await LoadTasks();

        CompleteTaskCommand = ReactiveCommand.Create<int>(async id => {
            var completeTaskResult = await _taskService.CompleteTask(id);
            if (!completeTaskResult.Success) {
                await DialogService.ShowErrorMessageBox(completeTaskResult.Message);
                return;
            }
            await LoadTasks();
        });
    }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<TaskEntity>? Tasks { get; set; }

    #endregion

    #region Commands

    public ReactiveCommand<int, Unit> CompleteTaskCommand { get; init; }

    #endregion

    #region Services

    private readonly NavigationService _navigationService;

    private readonly TaskService _taskService;

    #endregion

    #region Public Methods

    public async Task LoadTasks() {
        Tasks = await _taskService.Get();
    }

    public async Task OpenAddTaskWindow() {
        await _navigationService.ShowTaskWindowDialog(_eventHandlerClosedWindowDialog);
    }

    #endregion
}