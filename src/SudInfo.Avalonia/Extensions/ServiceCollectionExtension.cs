namespace SudInfo.Avalonia.Extensions;

internal static class ServiceCollectionExtension
{
    public static readonly IServiceProvider ServiceProvider = new ServiceCollection()

                                                              #region Page view models

                                                             .AddScoped<ComputersPageViewModel>()
                                                             .AddScoped<PrintersPageViewModel>()
                                                             .AddScoped<MonitorsPageViewModel>()
                                                             .AddScoped<UsersPageViewModel>()
                                                             .AddScoped<RutokensPageViewModel>()
                                                             .AddScoped<PeripheryPageViewModel>()
                                                             .AddScoped<WorkplacesPageViewModel>()
                                                             .AddScoped<ServersPageViewModel>()
                                                             .AddScoped<TasksPageViewModel>()
                                                             .AddScoped<PasswordsPageViewModel>()
                                                             .AddScoped<AppsPageViewModel>()
                                                             .AddScoped<ContactsPageViewModel>()
                                                             .AddScoped<CartridgesPageViewModel>()
                                                             .AddScoped<PhonesPageViewModel>()

                                                              #endregion

                                                              #region Window view models

                                                             .AddScoped<AppWindowViewModel>()
                                                             .AddScoped<CartridgeWindowViewModel>()
                                                             .AddScoped<ComputerWindowViewModel>()
                                                             .AddScoped<ContactWindowViewModel>()
                                                             .AddScoped<PasswordWindowViewModel>()
                                                             .AddScoped<TaskWindowViewModel>()
                                                             .AddScoped<ServerRackWindowViewModel>()
                                                             .AddScoped<PeripheryWindowViewModel>()
                                                             .AddScoped<ServerWindowViewModel>()
                                                             .AddScoped<RutokenWindowViewModel>()
                                                             .AddScoped<UserWindowViewModel>()
                                                             .AddScoped<MonitorWindowViewModel>()
                                                             .AddScoped<PrinterWindowViewModel>()
                                                             .AddScoped<PhoneWindowViewModel>()

                                                              #endregion

                                                              #region Services

                                                             .AddTransient<PrinterService>()
                                                             .AddTransient<RutokenService>()
                                                             .AddTransient<PeripheryService>()
                                                             .AddTransient<ServerService>()
                                                             .AddTransient<CartridgeService>()
                                                             .AddTransient<ContactService>()
                                                             .AddTransient<PasswordService>()
                                                             .AddTransient<AppService>()
                                                             .AddTransient<TaskService>()
                                                             .AddTransient<UserService>()
                                                             .AddTransient<MonitorService>()
                                                             .AddTransient<ServerRackService>()
                                                             .AddSingleton<NavigationService>()
                                                             .AddTransient<ComputerService>()
                                                             .AddTransient<PhoneService>()
                                                             .AddTransient<SudInfoDatabaseContext>()
                                                             .BuildServiceProvider();

    #endregion
}