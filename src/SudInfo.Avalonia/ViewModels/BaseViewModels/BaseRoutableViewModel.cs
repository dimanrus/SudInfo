namespace SudInfo.Avalonia.ViewModels.BaseViewModels;

public abstract class BaseRoutableViewModel : BaseViewModel, IRoutableViewModel
{
    #region Protected Variables

    protected EventHandler? EventHandlerClosedWindowDialog { get; init; }

    #endregion

    #region IRoutableViewModel Realization

    public string? UrlPathSegment { get; }

    public IScreen HostScreen => Locator.Current.GetService<IScreen>()!;

    #endregion
}