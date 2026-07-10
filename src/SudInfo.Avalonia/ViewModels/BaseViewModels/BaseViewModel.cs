namespace SudInfo.Avalonia.ViewModels.BaseViewModels;

public abstract class BaseViewModel : ReactiveObject
{
    public static bool ValidationModel<T>(T model) {
        if (model == null)
            return false;
        return Validator.TryValidateObject(model, new(model), [], true);
    }
}