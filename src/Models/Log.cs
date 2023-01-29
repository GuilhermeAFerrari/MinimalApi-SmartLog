using MinimalApi_SmartLog.Enums;

namespace MinimalApi_SmartLog.Models;

public class Log
{
    public int Id { get; set; }
    public Guid IdSecondary { get; set; }
    public string Message { get; set; } = null!;
    public string? StackTrace { get; set; }
    public string? Request { get; set; }
    public string? Response { get; set; }
    public Level Level { get; set; }
    public DateTime Date { get; set; }
    public bool Active { get; set; }
}

public class Counter
{
    public int High { get; set; }
    public int Middle { get; set; }
    public int Low { get; set; }
}