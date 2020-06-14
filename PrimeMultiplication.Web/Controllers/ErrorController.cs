namespace PrimeMultiplication.Web
{
    using System.Linq;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        [HttpGet("error")]
        public IActionResult Error()
        {
            var handler = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var pathComponents = handler.Path.Split("/");
            var count = pathComponents[2];
            var timeout = pathComponents.Last();

            return this.BadRequest($"Could not generate {count} primes in {timeout}ms. Try less.");
        }
    }
}
