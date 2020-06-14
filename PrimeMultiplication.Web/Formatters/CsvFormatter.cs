// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;
    using PrimeMultiplication;

    internal class CsvFormatter : TableFormatter
    {
        internal CsvFormatter()
            : base(new MediaTypeHeaderValue("text/csv"))
        {
        }

        protected override async Task WriteResponseBodyAsync(MultiplicationTable table, OutputFormatterWriteContext context, Encoding encoding, CancellationToken cancellationToken)
        {
            using var writer = context.WriterFactory(context.HttpContext.Response.Body, encoding);

            await foreach (var row in table.WithCancellation(cancellationToken))
            {
                var fieldNumber = 0;
                await foreach (var cell in row)
                {
                    if (fieldNumber++ > 0)
                    {
                        await writer.WriteAsync(",");
                    }

                    await writer.WriteAsync(cell?.ToString(CultureInfo.InvariantCulture));
                }

                await writer.WriteLineAsync();
            }

            await writer.FlushAsync();
        }
    }
}
