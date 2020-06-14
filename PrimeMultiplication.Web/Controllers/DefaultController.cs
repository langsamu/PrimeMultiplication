namespace PrimeMultiplication.Web
{
    using System.Threading;
    using Microsoft.AspNetCore.Mvc;
    using PrimeMultiplication;

    [Route("multiply")]
    public class DefaultController : Controller
    {
        [HttpGet("{count}")]
        [HttpGet("{count}/stop-after/{timeout}")]
        public IActionResult Default(UiParameters parameters) =>
            this.View(parameters);

        [HttpGet("{count}/fail-after/{timeout}")]
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
