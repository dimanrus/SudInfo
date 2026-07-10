namespace SudInfo.Avalonia.ViewModels.WindowViewModels;

public class TaskWindowViewModel(
    NavigationService navigationService,
    TaskService taskService) : BaseViewModel
{
    private Action _closedWindow;

    public TaskEntity Task { get; set; } = new();

    public DateTimeOffset ReminderDate { get; set; } = DateTimeOffset.Now;

    public TimeSpan ReminderTime { get; set; } = DateTime.Now.TimeOfDay;

    public void Initialization(Action close) {
        _closedWindow = close;
    }

    public async Task AddTask() {
        if (!ValidationModel(Task))
            return;
        Task.ReminderTime = new(ReminderDate.Year, ReminderDate.Month, ReminderDate.Day, ReminderTime.Hours, ReminderTime.Minutes, 0);
        var addTaskResult = await _taskService.Add(Task);
        if (!addTaskResult.Success) {
            await DialogService.ShowErrorMessageBox(addTaskResult.Message);
            return;
        }
        _closedWindow();
    }

    #region Services

    private readonly NavigationService _navigationService = navigationService;

    private readonly TaskService _taskService = taskService;

    #endregion
}