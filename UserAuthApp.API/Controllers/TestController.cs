using Microsoft.AspNetCore.Mvc;

namespace UserAuthApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { 
            Status = "Healthy", 
            Timestamp = DateTime.UtcNow,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
        });
    }

    [HttpGet("google-test")]
    public IActionResult GoogleTest()
    {
        var googleClientId = HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()["Google:ClientId"];
            
        return Ok(new { 
            GoogleConfigured = !string.IsNullOrEmpty(googleClientId),
            ClientIdPresent = !string.IsNullOrEmpty(googleClientId),
            RedirectUri = Url.Action("GoogleCallback", "Auth")
        });
    }
}