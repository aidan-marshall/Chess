using Microsoft.AspNetCore.Mvc;

namespace Chess.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok(new { message = "Pong" });
    }
}
