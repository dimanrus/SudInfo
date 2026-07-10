namespace SudInfo.Avalonia.Services;

public class PrinterService(
    SudInfoDatabaseContext context) : BaseService<Printer>(context)
{
    public async override Task<Result> Add(Printer printer) {
        try {
            if (printer.Computer != null) {
                printer.Computer = await context.Computers.AsNoTracking()
                                                .FirstAsync(x => x.Id == printer.Computer.Id);
            }
            context.Entry(printer).State = EntityState.Added;
            await context.Printers.AddAsync(printer);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async Task<Result> Remove(int id) {
        try {
            var printer = await context.Printers.AsNoTracking()
                                       .FirstAsync(x => x.Id == id);
            context.Entry(printer).State = EntityState.Deleted;
            context.Printers.Remove(printer);
            await context.SaveChangesAsync();
            return new(true);
        }
        catch (Exception ex) {
            return new(message: ex.Message);
        }
    }

    public async override Task<Result> Update(Printer printer) {
        /*        try
                {*/
        var entity = await context.Printers.Include(x => x.Computer)
                                  .FirstAsync(x => x.Id == printer.Id);
        entity.Computer = printer.Computer;
        await context.SaveChangesAsync();
        return new(true);
        /*        }
                catch (Exception ex)
                {
                    return new()
                    {
                        Message = ex.Message
                    };
                }*/
    }

    #region Get Methods

    public async Task<Result<Printer>> Get(int id) {
        try {
            var printer = await context.Printers.Include(x => x.Computer)
                                       .FirstOrDefaultAsync(x => x.Id == id);
            return printer == null ? throw new("Принтер не найден.") : new(printer, true);
        }
        catch (Exception ex) {
            return new(null, message: ex.Message);
        }
    }

    public async Task<IReadOnlyCollection<Printer>> Get() {
        return await context.Printers.AsNoTracking()
                            .Include(static x => x.Computer)
                            .ThenInclude(static x => x.User)
                            .ToListAsync();
    }

    #endregion
}