namespace WebApplication1.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    [Route("")]
    public class Default : Controller
    {
        [HttpGet("{thisMany}")]
        public IActionResult Index(int thisMany) =>
            this.View(thisMany.Primes().ToArray());
    }
}
