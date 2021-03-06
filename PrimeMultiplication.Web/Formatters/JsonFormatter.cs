﻿// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Net.Mime;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;
    using PrimeMultiplication;

    internal class JsonFormatter : TableFormatter
    {
        internal JsonFormatter()
            : base(new MediaTypeHeaderValue(MediaTypeNames.Application.Json))
        {
        }

        protected override async Task WriteResponseBodyAsync(MultiplicationTable table, OutputFormatterWriteContext context, Encoding encoding, CancellationToken cancellationToken)
        {
            using var writer = new Utf8JsonWriter(context.HttpContext.Response.Body);

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
            await writer.FlushAsync();
        }
    }
}
