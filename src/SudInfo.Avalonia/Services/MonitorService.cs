namespace SudInfo.Avalonia.Services;

public class MonitorService(
    SudInfoDatabaseContext context) : BaseService<Monitor>(context)
{
    public async Task<Result> Remove(int id) {
        try {
            var monitor = await context.Monitors.AsNoTracking()
                                       .FirstAsync(x => x.Id == id);
            context.Entry(monitor).State = EntityState.Deleted;
            context.Monitors.Remove(monitor);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async override Task<Result> Add(Monitor monitor) {
        try {
            if (monitor.Computer != null) {
                monitor.Computer = await context.Computers.AsNoTracking()
                                                .FirstAsync(x => x.Id == monitor.Computer.Id);
            }
            context.Entry(monitor).State = EntityState.Added;
            await context.Monitors.AddAsync(monitor);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async override Task<Result> Update(Monitor monitor) {
        try {
            var entity = await context.Monitors.Include(x => x.Computer)
                                      .FirstAsync(x => x.Id == monitor.Id);
            entity.Computer = monitor.Computer;
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new() {
                Message = ex.Message
            };
        }
    }

    #region Get Methods

    public async Task<IReadOnlyCollection<Monitor>> Get() {
        return await context.Monitors.AsNoTracking()
                            .Include(static x => x.Computer)
                            .ThenInclude(static x => x.User)
                            .ToListAsync();
    }

    public async Task<Result<Monitor>> Get(int id) {
        try {
            var monitor = await context.Monitors.Include(x => x.Computer)
                                       .ThenInclude(x => x.User)
                                       .FirstOrDefaultAsync(x => x.Id == id);
            return monitor == null ? throw new("Computer not Found") : new(monitor, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    #endregion
}