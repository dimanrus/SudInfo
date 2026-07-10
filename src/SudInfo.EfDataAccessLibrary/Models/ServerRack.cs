namespace SudInfo.EfDataAccessLibrary.Models;

[Index(nameof(Position), IsUnique = true)]
public class ServerRack : BaseModel
{
    [XLColumn(Ignore = true)]
    public int Position { get; set; } = 1;

    [XLColumn(Header = "Инвентарный номер")]
    [StringLength(50)]
    public string? InventarNumber { get; set; }

    public List<Server> Servers { get; set; } = [];
}