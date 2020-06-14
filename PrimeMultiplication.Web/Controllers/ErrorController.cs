// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Linq;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents a friendly error page.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// An endpoint that generates a friendly error page.
        /// </summary>
        /// <returns>HTTP status code 400.</returns>
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
