namespace SudInfo.EfDataAccessLibrary.Models;

public class Server : BaseModel
{
    [XLColumn(Header = "Название")]
    [Required(ErrorMessage = Const.FieldRequired)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = Const.LengthMore2)]
    public string? Name { get; set; }

    [XLColumn(Header = "Серийный номер")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = Const.LengthMore2)]
    public string? SerialNumber { get; set; }

    [XLColumn(Header = "Инвентарный номер")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = Const.LengthMore2)]
    public string? InventarNumber { get; set; }

    [XLColumn(Ignore = true)]
    public int? PosiitionInServerRack { get; set; }

    [XLColumn(Header = "IP Адрес")]
    [RegularExpression(Const.Ip4RegularExpression, ErrorMessage = Const.NotValidIp4Message)]
    [StringLength(15)]
    public string? IpAddress { get; set; }

    [XLColumn(Ignore = true)]
    public int? ServerRackId { get; set; }

    public ServerRack? ServerRack { get; set; }

    [XLColumn(Header = "Операционная система")]
    public ServerOperatingSystem OperatingSystem { get; set; } = ServerOperatingSystem.WindowsServer2003;
}

public enum ServerOperatingSystem
{
    WindowsServer2012, WindowsServer2003, WindowsServer2008, WindowsServer2016, WindowsServer2019, UbuntuServer, ESXi
}