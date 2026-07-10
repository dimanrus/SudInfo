namespace SudInfo.Avalonia.Services;

public static class DialogService
{
    public async static Task ShowErrorMessageBox(string message) {
        await MessageBoxManager.GetMessageBoxStandard("Ошибка",
                                                      message,
                                                      icon: Icon.Error)
                               .ShowAsync();
    }

    public async static Task<ButtonResult> ShowQuestionMessageBox(string message) {
        return await MessageBoxManager.GetMessageBoxStandard("Сообщение",
                                                             message,
                                                             ButtonEnum.YesNo,
                                                             Icon.Question)
                                      .ShowAsync();
    }
}