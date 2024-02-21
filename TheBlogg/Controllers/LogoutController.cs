using Microsoft.AspNetCore.Mvc;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "success" });
        }
    }
}
