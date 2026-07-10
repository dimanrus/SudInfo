namespace SudInfo.EfDataAccessLibrary;

public static class Const
{
    public const string EmptyWithSpaceString = " ";

    public const string LengthMore2 = "Длина должна быть больше 2.";

    public const string FieldRequired = "Требуется к заполнению.";

    public const string NotValidIp4Message = "Не соответствует формату IP4.";

    public const string NotValidPhoneMessage = "А это не номер трубки, может другой? :)";

    public const string Ip4RegularExpression = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

    public const string NumberCannotBeGreaterThen0Message = "Значение не может быть равно 0.";
}