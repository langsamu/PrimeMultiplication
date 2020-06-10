namespace WebApplication1.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    [Route("")]
    public class Default : Controller
    {
        private const int timeout = 1000;

        [HttpGet("{thisMany}")]
        public IActionResult Index(int thisMany) =>
            this.View(thisMany.Primes().ToArray());

        [HttpGet("async/{thisMany}")]
        public async Task<IActionResult> Cancellable(int thisMany, CancellationToken cancellationToken)
        {
            var timeoutCancellation = new CancellationTokenSource(timeout);
            var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellation.Token);

            this.Response.RegisterForDispose(timeoutCancellation);
            this.Response.RegisterForDispose(linkedCancellation);

            return this.View(thisMany.PrimesAsync(linkedCancellation.Token));
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            var handler = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (handler.Path.StartsWith("/async/") && handler.Error is TaskCanceledException)
            {
                return this.BadRequest($"Couldn't generate {handler.Path[7..]} primes in {timeout}ms. Try less.");
            }
            else
            {
                throw handler.Error;
            }
        }
    }
}
