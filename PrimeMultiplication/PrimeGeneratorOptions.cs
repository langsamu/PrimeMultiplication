// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication
{
    using System;

    /// <summary>
    /// Options for how to generate primes.
    /// </summary>
    [Flags]
    public enum PrimeGeneratorOptions
    {
        /// <summary>
        /// Default options
        /// </summary>
        None = 0b_0,

        /// <summary>
        /// Throw an exception if the operation is cancelled
        /// </summary>
        ThrowOnCancel = 0b_1,
    }
}
