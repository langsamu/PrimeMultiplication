// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Microsoft.AspNetCore.Mvc;
    using PrimeMultiplication;

    /// <summary>
    /// Represents the UI part of the web app.
    /// </summary>
    [Route("multiply")]
    public class MultiplyController : Controller
    {
        /// <summary>
        /// An endpoint that generates prime multiplication tables as an HTML table.
        /// </summary>
        /// <param name="parameters">Request parameters.</param>
        /// <returns>An view that serialises the parameters as an HTML table.</returns>
        /// <remarks>Processing stops and the partial response generated hitherto is returned in a well-formed manner.</remarks>
        [HttpGet("{count}")]
        [HttpGet("{count}/stop-after/{timeout}")]
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Provided by framework")]
        public IActionResult Default(UiParameters parameters) =>
            this.View(parameters);

        /// <summary>
        /// An endpoint that generates prime multiplication tables as an HTML table.
        /// </summary>
        /// <param name="parameters">Request parameters.</param>
        /// <returns>An view that serialises the parameters as an HTML table.</returns>
        /// <remarks>An exception is thrown if time limit is exceeded.</remarks>
        [HttpGet("{count}/fail-after/{timeout}")]
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Provided by framework")]
        public IActionResult AbruptTimeout(UiParameters parameters)
        {
            parameters.ThrowOnCancel = true;
            return this.View(parameters);
        }

        private IActionResult View(UiParameters parameters)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var options = PrimeGeneratorOptions.None;
            if (parameters.ThrowOnCancel)
            {
                options = PrimeGeneratorOptions.ThrowOnCancel;
            }

            if (parameters.Timeout.HasValue)
            {
                var tokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(
                        parameters.CancellationToken,
                        new CancellationTokenSource(parameters.Timeout.Value).Token,
                        this.HttpContext.RequestAborted);

                this.Response.RegisterForDispose(tokenSource);

                parameters.CancellationToken = tokenSource.Token;
            }

            var model = (
                Table: new MultiplicationTable(parameters.Count, options),
                Timeout: parameters.CancellationToken);

            return this.View("Index", model);
        }
    }
}
