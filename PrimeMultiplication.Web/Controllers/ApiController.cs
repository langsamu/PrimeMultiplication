// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents the API part of the web app.
    /// </summary>
    [ApiController]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// An endpoint that generates prime multiplication tables in various formats.
        /// </summary>
        /// <param name="parameters">API parameters.</param>
        /// <remarks>The single endpoint for this API features content negotiation based on HTTP Accept header or 'file extensions', and can be cancelled.</remarks>
        /// <returns>An <see cref="OkObjectResult"/> that will be serialised based on content negotiation.</returns>
        [HttpGet("api")]
        [HttpGet("api.{format}")]
        [FormatFilter]
        public IActionResult Get([FromQuery] ApiParameters parameters) =>
            this.Ok(parameters);
    }
}
