using Monitor = SudInfo.EfDataAccessLibrary.Models.Monitor;
using TaskEntity = SudInfo.EfDataAccessLibrary.Models.TaskEntity;

namespace SudInfo.EfDataAccessLibrary.Contexts;

public class SudInfoDatabaseContext : DbContext
{
    #region Ctors

    public SudInfoDatabaseContext() {
        Database.Migrate();
    }

    #endregion

    #region Configuration

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseSqlite("Data Source = SudInfoDatabase.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AppSetting>().HasData(new AppSetting {
            Id = 1,
            Theme = "Light"
        });
    }

    #endregion

    #region Tables

    public DbSet<User> Users => Set<User>();

    public DbSet<Computer> Computers => Set<Computer>();

    public DbSet<Printer> Printers => Set<Printer>();

    public DbSet<Monitor> Monitors => Set<Monitor>();

    public DbSet<Rutoken> Rutokens => Set<Rutoken>();

    public DbSet<Periphery> Peripheries => Set<Periphery>();

    public DbSet<Server> Servers => Set<Server>();

    public DbSet<ServerRack> ServerRacks => Set<ServerRack>();

    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();

    public DbSet<PasswordEntity> Passwords => Set<PasswordEntity>();

    public DbSet<AppEntity> Apps => Set<AppEntity>();

    public DbSet<Contact> Contacts => Set<Contact>();

    public DbSet<Cartridge> Cartridges => Set<Cartridge>();

    public DbSet<AppSetting> AppSettings => Set<AppSetting>();

    public DbSet<Phone> Phones => Set<Phone>();

    #endregion
}