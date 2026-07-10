namespace SudInfo.Avalonia.Services;

public class PasswordService(
    SudInfoDatabaseContext context) : BaseService<PasswordEntity>(context)
{
    public async Task<Result> Remove(int id) {
        try {
            var passwordEntity = await context.Passwords.AsNoTracking()
                                              .FirstAsync(x => x.Id == id);
            context.Entry(passwordEntity).State = EntityState.Deleted;
            context.Passwords.Remove(passwordEntity);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    #region Get Methods

    public async Task<IReadOnlyCollection<PasswordEntity>> Get() {
        return await context.Passwords.AsNoTracking().ToListAsync();
    }

    public async Task<Result<PasswordEntity>> Get(int id) {
        try {
            var server = await context.Passwords.FirstAsync(x => x.Id == id);
            return new(server, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    #endregion
}