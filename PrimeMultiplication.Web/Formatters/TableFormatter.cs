// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;
    using PrimeMultiplication;

    internal abstract class TableFormatter : TextOutputFormatter
    {
        protected TableFormatter(MediaTypeHeaderValue mimeType)
        {
            this.SupportedMediaTypes.Add(mimeType);
            this.SupportedEncodings.Add(new UTF8Encoding(false));
        }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Out of scope for this demo")]
        public sealed override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding encoding)
        {
            var data = (ApiParameters)context.Object;

            var table = new MultiplicationTable(data.Count);

            var cancellationToken =
               CancellationTokenSource.CreateLinkedTokenSource(
                   data.CancellationToken,
                   context.HttpContext.RequestAborted).Token;

            if (data.Timeout.HasValue)
            {
                cancellationToken =
                    CancellationTokenSource.CreateLinkedTokenSource(
                        cancellationToken,
                        new CancellationTokenSource(data.Timeout.Value).Token).Token;
            }

            await this.WriteResponseBodyAsync(table, context, encoding, cancellationToken);
        }

        protected abstract Task WriteResponseBodyAsync(MultiplicationTable table, OutputFormatterWriteContext context, Encoding encoding, CancellationToken cancellationToken);

        protected sealed override bool CanWriteType(Type type) =>
            typeof(ApiParameters).IsAssignableFrom(type);
    }
}
