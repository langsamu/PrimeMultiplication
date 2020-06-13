namespace WebApplication1
{
    using System;
    using System.Net;
    using System.Net.Mime;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassLibrary1;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;

    internal class JsonFormatter : TableFormatter
    {
        internal JsonFormatter()
            : base(new MediaTypeHeaderValue(MediaTypeNames.Application.Json))
        {
        }

        protected override async Task WriteResponseBodyAsync(MultiplicationTable table, CancellationToken cancellationToken, OutputFormatterWriteContext context, Encoding encoding)
        {
            using var writer = new Utf8JsonWriter(context.HttpContext.Response.Body);

            try
            {
                writer.WriteStartArray();

                await foreach (var row in table.WithCancellation(cancellationToken))
                {
                    writer.WriteStartArray();

                    await foreach (var cell in row)
                    {
                        if (cell.HasValue)
                        {
                            writer.WriteNumberValue(cell.Value);
                        }
                        else
                        {
                            writer.WriteNullValue();
                        }
                    }

                    writer.WriteEndArray();
                }

                writer.WriteEndArray();
            }
            catch (OperationCanceledException)
            {
                writer.WriteStartObject();
                writer.WriteString("error", "Could not multiply primes in time limit. Try less.");
                writer.WriteEndObject();

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            await writer.FlushAsync();
        }
    }
}
