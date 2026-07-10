namespace SudInfo.EfDataAccessLibrary.Models;

public class Periphery : BaseModel
{
    [XLColumn(Header = "Наименование")]
    [Required(ErrorMessage = Const.FieldRequired)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = Const.LengthMore2)]
    public string? Name { get; set; }

    [XLColumn(Header = "Тип")]
    public PeripheryType Type { get; set; }

    [XLColumn(Header = "Серийный номер")]
    [Required(ErrorMessage = Const.FieldRequired)]
    [StringLength(50, MinimumLength = 2, ErrorMessage = Const.LengthMore2)]
    public string? SerialNumber { get; set; }

    [XLColumn(Header = "Инвентарный номер")]
    [StringLength(50)]
    public string? InventarNumber { get; set; }

    [XLColumn(Header = "Личное")]
    public bool IsPersonal { get; set; }

    [XLColumn(Header = "Сломан")]
    public bool IsBroken { get; set; }

    [XLColumn(Header = "Описание поломки")]
    [StringLength(200)]
    public string? BreakdownDescription { get; set; }

    public Computer? Computer { get; set; }

    [XLColumn(Ignore = true)]
    [NotMapped]
    public string Icon => Type switch {
        PeripheryType.Мышь => "avares://SudInfo.Avalonia/Assets/Images/mouse.png",
        PeripheryType.Наушкники => "avares://SudInfo.Avalonia/Assets/Images/headphones.png",
        PeripheryType.Микрофон => "avares://SudInfo.Avalonia/Assets/Images/microphone.png",
        PeripheryType.Колонки => "avares://SudInfo.Avalonia/Assets/Images/sound.png",
        PeripheryType.Клавиатура => "avares://SudInfo.Avalonia/Assets/Images/keyboard.png",
        PeripheryType.Сканер => "avares://SudInfo.Avalonia/Assets/Images/scanner.png",
        _ => "avares://SudInfo.Avalonia/Assets/Images/battery.png"
    };
}

public enum PeripheryType
{
    Мышь, Клавиатура, Микрофон, Сканер, Колонки, Наушкники, ИБП
}