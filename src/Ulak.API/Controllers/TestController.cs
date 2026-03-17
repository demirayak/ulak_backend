using Microsoft.AspNetCore.Mvc;
using Serilog;
using Ulak.Application.Common.Responses;

namespace Ulak.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("log-demo")]
    public IActionResult LogDemo()
    {
        Log.Information("Log demo endpoint çağrıldı");
        Log.Warning("Warning örneği");
        Log.Error("Error örneği");

        return Ok("Serilog ile loglama yapıldı");
    }

    [HttpGet("test-log")]
    public IActionResult TestLog()
    {
        _logger.LogInformation("Test log endpoint çağrıldı");
        _logger.LogWarning("Bu bir warning örneği");
        _logger.LogError("Bu bir hata logu örneği");

        return Ok("Loglar console'a yazıldı");
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Test hata");
    }

    [HttpGet("test-success")]
    public ApiResponse<string> Get()
    {
        return ApiResponse<string>.Ok("çalıştı");
    }
}