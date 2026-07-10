namespace SudInfo.Avalonia.Services;

public class PhoneService(
    SudInfoDatabaseContext context) : BaseService<Phone>(context)
{
    public async override Task<Result> Add(Phone phone) {
        try {
            if (phone.User != null) {
                phone.User = await context.Users.AsNoTracking()
                                          .FirstAsync(x => x.Id == phone.User.Id);
            }
            context.Entry(phone).State = EntityState.Added;
            await context.AddAsync(phone);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async Task<Result> Remove(int id) {
        try {
            var phone = await context.Phones.AsNoTracking()
                                     .FirstAsync(x => x.Id == id);
            context.Entry(phone).State = EntityState.Deleted;
            context.Remove(phone);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    #region Get methods

    public async Task<Result<Phone>> Get(int id) {
        try {
            var phone = await context.Phones.Include(x => x.User)
                                     .FirstAsync(x => x.Id == id);
            return phone == null ? throw new("Телефон не найден") : new(phone, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    public async Task<IReadOnlyCollection<Phone>> Get() {
        return await context.Phones.AsNoTracking()
                            .Include(static x => x.User)
                            .ToListAsync();
    }

    #endregion
}