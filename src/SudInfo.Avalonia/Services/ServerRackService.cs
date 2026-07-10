namespace SudInfo.Avalonia.Services;

public class ServerRackService(
    SudInfoDatabaseContext context) : BaseService<ServerRack>(context)
{
    public async Task<Result> Remove(int id) {
        try {
            var serverRack = await context.ServerRacks.AsNoTracking()
                                          .FirstAsync(x => x.Id == id);
            context.Entry(serverRack).State = EntityState.Deleted;
            context.Remove(serverRack);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    #region Get Methods

    public async Task<Result<IReadOnlyCollection<ServerRack>>> Get() {
        try {
            var serverRacks = await context.ServerRacks.AsNoTracking()
                                           .Include(static x => x.Servers)
                                           .ToListAsync();
            foreach (var serverRack in serverRacks) {
                serverRack.Servers = [.. serverRack.Servers.OrderBy(static x => x.PosiitionInServerRack)];
            }
            return new(serverRacks, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    public async Task<Result<ServerRack>> Get(int id) {
        try {
            var serverRack = await context.ServerRacks.FirstAsync(x => x.Id == id);
            return new(serverRack, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    public async Task<int> GetNumberServerRacks() {
        return await context.ServerRacks.CountAsync();
    }

    #endregion
}