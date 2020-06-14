namespace WebApplication1
{
    using System;
    using System.Linq;
    using System.Threading;
    using ClassLibrary1;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        [HttpGet("error")]
        public IActionResult Error()
        {
            var handler = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var pathComponents = handler.Path.Split("/");
            var count = pathComponents[1];
            var timeout = pathComponents.Last();

            return this.BadRequest($"Could not generate {count} primes in {timeout}ms. Try less.");
        }
    }
}
