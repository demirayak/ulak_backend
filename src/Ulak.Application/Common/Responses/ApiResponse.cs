namespace Ulak.Application.Common.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; }

    public T? Data { get; set; }

    public string? Message { get; set; }

    public object? Errors { get; set; }

    public string? TraceId { get; set; }

    public static ApiResponse<T> Ok(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
    }
}

public class ApiResponse
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public object? Errors { get; set; }

    public string? TraceId { get; set; }

    public static ApiResponse Ok()
    {
        return new ApiResponse
        {
            Success = true
        };
    }

    public static ApiResponse Fail(string message)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message
        };
    }
}