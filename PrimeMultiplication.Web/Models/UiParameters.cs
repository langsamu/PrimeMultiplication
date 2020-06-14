// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    /// <summary>
    /// Represents parameters used in UI requests.
    /// </summary>
    public class UiParameters : ApiParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether to throw an exception if time limit is exceeded or just stop.
        /// </summary>
        public bool ThrowOnCancel { get; set; }
    }
}
