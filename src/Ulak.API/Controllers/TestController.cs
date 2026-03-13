using Microsoft.AspNetCore.Mvc;
using Ulak.Application.Common.Responses;

namespace Ulak.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
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