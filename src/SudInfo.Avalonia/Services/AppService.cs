namespace SudInfo.Avalonia.Services;

public class AppService(
    SudInfoDatabaseContext context) : BaseService<AppEntity>(context)
{
    public async Task<Result> Remove(int id) {
        try {
            var entity = await context.Apps.AsNoTracking()
                                      .Include(x => x.Computers)
                                      .FirstAsync(x => x.Id == id);
            entity.Computers!.Clear();
            context.Entry(entity).State = EntityState.Deleted;
            context.Apps.Remove(entity);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async override Task<Result> Update(AppEntity entity) {
        try {
            var app = await context.Apps.Include(x => x.Computers)
                                   .FirstAsync(x => x.Id == entity.Id);
            app.Name = entity.Name;
            app.Version = entity.Version;
            if (entity.Computers!.Count == 0) {
                app.Computers = null;
            }
            else {
                app.Computers = [];
                foreach (var computer in entity.Computers) {
                    app.Computers.Add(await context.Computers.FindAsync(computer.Id));
                }
            }
            context.Update(app);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async override Task<Result> Add(AppEntity entity) {
        try {
            var computers = entity.Computers;
            entity.Computers = [];
            foreach (var computer in computers) {
                entity.Computers.Add(await context.Computers.FindAsync(computer.Id));
            }
            context.Entry(entity).State = EntityState.Added;
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    #region Get Methods

    public async Task<IReadOnlyCollection<AppEntity>> Get() {
        return await context.Apps.AsNoTracking()
                            .Include(static x => x.Computers)
                            .ThenInclude(static x => x.User)
                            .ToListAsync();
    }

    public async Task<Result<AppEntity>> Get(int id) {
        try {
            var server = await context.Apps
                                      .Include(x => x.Computers)
                                      .FirstAsync(x => x.Id == id);
            return new(server, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    #endregion
}