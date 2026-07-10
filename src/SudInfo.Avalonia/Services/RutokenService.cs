namespace SudInfo.Avalonia.Services;

public class RutokenService(
    SudInfoDatabaseContext context) : BaseService<Rutoken>(context)
{
    public async Task<Result> Remove(int id) {
        try {
            var rutoken = await context.Rutokens.AsNoTracking()
                                       .FirstAsync(x => x.Id == id);
            context.Entry(rutoken).State = EntityState.Deleted;
            context.Rutokens.Remove(rutoken);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async override Task<Result> Add(Rutoken rutoken) {
        try {
            if (rutoken.User != null) {
                rutoken.User = await context.Users.AsNoTracking()
                                            .FirstAsync(x => x.Id == rutoken.User.Id);
            }
            context.Entry(rutoken).State = EntityState.Added;
            await context.Rutokens.AddAsync(rutoken);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    #region Get Methods

    public async Task<Result<Rutoken>> Get(int id) {
        try {
            var rutoken = await context.Rutokens.Include(x => x.User)
                                       .FirstAsync(x => x.Id == id);
            return rutoken == null ? throw new("ЭЦП не найден") : new(rutoken, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    public async Task<IReadOnlyCollection<Rutoken>> Get() {
        return await context.Rutokens
                            .Include(static x => x.User)
                            .ToListAsync();
    }

    #endregion
}