// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents the home page.
    /// </summary>
    [Route("")]
    public class DefaultController : Controller
    {
        /// <summary>
        /// Represents the GET action.
        /// </summary>
        /// <returns>The default view.</returns>
        [HttpGet]
        public IActionResult Index() =>
            this.View();
    }
}
