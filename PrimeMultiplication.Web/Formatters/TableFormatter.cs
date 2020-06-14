namespace PrimeMultiplication.Web
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using PrimeMultiplication;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;

    internal abstract class TableFormatter : TextOutputFormatter
    {
        protected TableFormatter(MediaTypeHeaderValue mimeType)
        {
            this.SupportedMediaTypes.Add(mimeType);
            this.SupportedEncodings.Add(new UTF8Encoding(false));
        }

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

            await this.WriteResponseBodyAsync(table, cancellationToken, context, encoding);
        }

        protected abstract Task WriteResponseBodyAsync(MultiplicationTable table, CancellationToken cancellationToken, OutputFormatterWriteContext context, Encoding encoding);

        protected sealed override bool CanWriteType(Type type) =>
            typeof(ApiParameters).IsAssignableFrom(type);
    }
}
