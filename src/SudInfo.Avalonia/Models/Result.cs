namespace SudInfo.Avalonia.Models;

public class Result<T>(
    T? obj,
    bool success = false,
    string message = "") : Result(success, message)
{
    public T? Object { get; set; } = obj;
}

public class Result(
    bool success = false,
    string message = "")
{
    public bool Success { get; set; } = success;

    public string Message { get; set; } = message;
}