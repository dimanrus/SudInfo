namespace SudInfo.Avalonia.Services;

public class UserService(
    SudInfoDatabaseContext context) : BaseService<User>(context)
{
    public async Task<Result> Remove(int id) {
        try {
            var user = await context.Users.AsNoTracking()
                                    .FirstAsync(x => x.Id == id);
            context.Entry(user).State = EntityState.Deleted;
            context.Remove(user);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    #region Get Methods

    public async Task<Result<User>> Get(int id) {
        try {
            var user = await context.Users.FirstAsync(x => x.Id == id);
            return new(user, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    public async Task<IReadOnlyCollection<User>> Get() {
        return await context.Users.AsNoTracking()
                            .Include(static x => x.Computers)
                            .ThenInclude(static x => x.Monitors)
                            .Include(static x => x.Computers)
                            .ThenInclude(static x => x.Printers)
                            .Include(static x => x.Computers)
                            .ThenInclude(static x => x.Peripheries)
                            .ToListAsync();
    }

    #endregion
}