// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading;

    /// <summary>
    /// Represents parameters used in API calls.
    /// </summary>
    public class ApiParameters
    {
        /// <summary>
        /// Gets or sets the size of the multiplication table to generate.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the time limit to generate within.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the asynchronous cancellation token of the request.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
