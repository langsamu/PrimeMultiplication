// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;
    using PrimeMultiplication;

    internal class XmlFormatter : TableFormatter
    {
        internal XmlFormatter()
            : base(new MediaTypeHeaderValue(MediaTypeNames.Text.Xml))
        {
        }

        protected override async Task WriteResponseBodyAsync(MultiplicationTable table, OutputFormatterWriteContext context, Encoding encoding, CancellationToken cancellationToken)
        {
            using var writer = XmlWriter.Create(context.WriterFactory(context.HttpContext.Response.Body, encoding), new XmlWriterSettings { Async = true });

            await writer.WriteStartElementAsync(null, "table", null);

            await foreach (var row in table.WithCancellation(cancellationToken))
            {
                await writer.WriteStartElementAsync(null, "row", null);

                await foreach (var cell in row)
                {
                    await writer.WriteElementStringAsync(null, "cell", null, cell.ToString());
                }

                await writer.WriteEndElementAsync();
            }

            await writer.WriteEndElementAsync();

            await writer.FlushAsync();
        }
    }
}
