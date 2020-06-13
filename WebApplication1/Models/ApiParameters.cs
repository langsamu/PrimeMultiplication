namespace WebApplication1
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading;

    public class ApiParameters
    {
        [Range(1, int.MaxValue)]
        public int Count { get; set; }

        [Range(1, int.MaxValue)]
        public int? Timeout { get; set; }

        public bool ThrowOnCancel { get; set; }

        public CancellationToken cancellationToken { get; set; }
    }
}