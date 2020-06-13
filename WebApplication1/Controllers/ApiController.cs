namespace WebApplication1
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("api")]
        [HttpGet("api.{format}")]
        [FormatFilter]
        public IActionResult Get([FromQuery] ApiParameters p) =>
            this.Ok(p);
    }
}