namespace SudInfo.Avalonia.Services;

public class TaskService(
    SudInfoDatabaseContext context) : BaseService<TaskEntity>(context)
{
    public async Task<IReadOnlyCollection<TaskEntity>> Get() {
        return await context.Tasks.AsNoTracking()
                            .ToListAsync();
    }

    public async Task<Result> CompleteTask(int id) {
        try {
            var task = await context.Tasks.AsNoTracking()
                                    .FirstAsync(x => x.Id == id);
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }
}