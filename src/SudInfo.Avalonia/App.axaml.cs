namespace SudInfo.Avalonia;

public class App : Application
{
    #region Properties

    public static Window? MainWindow { get; private set; }

    #endregion

    #region Initialization

    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
        this.AttachDeveloperTools();
    }

    public async override void OnFrameworkInitializationCompleted() {
        MainWindow mainWindow = new();

        #region Load ThemeVariant

        await using SudInfoDatabaseContext context = new();
        var appSetting = await context.AppSettings.AsNoTracking()
                                      .FirstAsync();
        RequestedThemeVariant = appSetting.Theme switch {
            "Dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Light
        };

        #endregion

        #region Locator Pages Initialization

        Locator.CurrentMutable.RegisterConstant<IScreen>(new MainWindowViewModel());
        Locator.CurrentMutable.Register<IViewFor<ComputersPageViewModel>>(static () => new ComputersPage());
        Locator.CurrentMutable.Register<IViewFor<PrintersPageViewModel>>(static () => new PrintersPage());
        Locator.CurrentMutable.Register<IViewFor<MonitorsPageViewModel>>(static () => new MonitorsPage());
        Locator.CurrentMutable.Register<IViewFor<UsersPageViewModel>>(static () => new UsersPage());
        Locator.CurrentMutable.Register<IViewFor<RutokensPageViewModel>>(static () => new RutokensPage());
        Locator.CurrentMutable.Register<IViewFor<PeripheryPageViewModel>>(static () => new PeripheryPage());
        Locator.CurrentMutable.Register<IViewFor<WorkplacesPageViewModel>>(static () => new WorkplacesPage());
        Locator.CurrentMutable.Register<IViewFor<ServersPageViewModel>>(static () => new ServersPage());
        Locator.CurrentMutable.Register<IViewFor<TasksPageViewModel>>(static () => new TasksPage());
        Locator.CurrentMutable.Register<IViewFor<PasswordsPageViewModel>>(static () => new PasswordsPage());
        Locator.CurrentMutable.Register<IViewFor<AppsPageViewModel>>(static () => new AppsPage());
        Locator.CurrentMutable.Register<IViewFor<ContactsPageViewModel>>(static () => new ContactsPage());
        Locator.CurrentMutable.Register<IViewFor<CartridgesPageViewModel>>(static () => new CartridgesPage());
        Locator.CurrentMutable.Register<IViewFor<PhonesPageViewModel>>(static () => new PhonesPage());

        #endregion

        mainWindow.DataContext = DataContext = Locator.Current.GetService<IScreen>();

        ServiceCollectionExtension.ServiceProvider.GetService<NavigationService>()?.SetWindow(mainWindow);

        mainWindow.Show();
        MainWindow = mainWindow;

        base.OnFrameworkInitializationCompleted();
    }

    #endregion
}