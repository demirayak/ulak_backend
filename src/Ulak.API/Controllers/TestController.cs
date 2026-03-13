using Microsoft.AspNetCore.Mvc;

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
}