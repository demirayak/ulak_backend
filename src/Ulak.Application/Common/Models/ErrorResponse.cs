namespace Ulak.Application.Common.Models;

public class ErrorResponse
{
    public string Message { get; set; } = "";

    public string? Detail { get; set; }

    public string? TraceId { get; set; }
}