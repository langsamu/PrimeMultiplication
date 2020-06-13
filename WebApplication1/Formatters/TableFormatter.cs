namespace WebApplication1
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassLibrary1;
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

            var options = data.ThrowOnCancel ?
                PrimeGeneratorOptions.ThrowOnCancel :
                PrimeGeneratorOptions.None;

            var table = new MultiplicationTable(data.Count, options);

            var cancellationToken =
               CancellationTokenSource.CreateLinkedTokenSource(
                   data.cancellationToken,
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
