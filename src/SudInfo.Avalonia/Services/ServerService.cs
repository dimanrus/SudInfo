namespace SudInfo.Avalonia.Services;

public class ServerService(
    SudInfoDatabaseContext context) : BaseService<Server>(context)
{
    public async Task<Result<Server>> Get(int id) {
        try {
            var server = await context.Servers.FirstAsync(x => x.Id == id);
            return new(server, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    public async Task<Result> Remove(int id) {
        try {
            var server = await context.Servers.AsNoTracking()
                                      .FirstAsync(x => x.Id == id);
            context.Entry(server).State = EntityState.Deleted;
            context.Remove(server);
            await context.SaveChangesAsync();
            return new() {
                Success = true
            };
        }
        catch (Exception ex) {
            return new() {
                Message = ex.Message
            };
        }
    }

    public async Task<Result> UpServerPositionInServerRack(int id) {
        var server = await context.Servers.AsNoTracking()
                                  .Include(x => x.ServerRack)
                                  .ThenInclude(x => x.Servers)
                                  .FirstAsync(x => x.Id == id);
        if (server.PosiitionInServerRack == 1)
            return new(message: "Сервер уже находится в самом вверху");
        var previousServer = await context.Servers.AsNoTracking()
                                          .FirstAsync(x => x.ServerRackId == server.ServerRackId &&
                                                           x.PosiitionInServerRack == server.PosiitionInServerRack - 1);
        previousServer.PosiitionInServerRack++;
        server.PosiitionInServerRack--;
        await context.SaveChangesAsync();
        return new(true);
    }

    public async Task<Result> DownServerPositionInServerRack(int id) {
        var server = await context.Servers.AsNoTracking()
                                  .Include(x => x.ServerRack)
                                  .ThenInclude(x => x.Servers)
                                  .FirstAsync(x => x.Id == id);
        if (server.PosiitionInServerRack == server.ServerRack?.Servers.Count)
            return new(message: "Сервер уже находится в самом низу");
        var nextServer = await context.Servers.AsNoTracking()
                                      .FirstAsync(x => x.ServerRackId == server.ServerRackId &&
                                                       x.PosiitionInServerRack == server.PosiitionInServerRack + 1);
        nextServer.PosiitionInServerRack--;
        server.PosiitionInServerRack++;
        await context.SaveChangesAsync();
        return new(true);
    }
}