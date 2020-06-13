namespace WebApplication1
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassLibrary1;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;

    internal class CsvFormatter : TableFormatter
    {
        internal CsvFormatter()
            : base(new MediaTypeHeaderValue("text/csv"))
        {
        }

        protected override async Task WriteResponseBodyAsync(MultiplicationTable table, CancellationToken cancellationToken, OutputFormatterWriteContext context, Encoding encoding)
        {
            using var writer = context.WriterFactory(context.HttpContext.Response.Body, encoding);

            try
            {
                await foreach (var row in table.WithCancellation(cancellationToken))
                {
                    var fieldNumber = 0;
                    await foreach (var cell in row)
                    {
                        if (fieldNumber++ > 0)
                        {
                            await writer.WriteAsync(",");
                        }

                        await writer.WriteAsync(cell?.ToString());
                    }

                    await writer.WriteLineAsync();
                }
            }
            catch (OperationCanceledException)
            {
                await writer.WriteAsync("# Could not multiply primes in time limit. Try less.");

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            await writer.FlushAsync();
        }
    }
}
