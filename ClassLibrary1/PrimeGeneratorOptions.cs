namespace ClassLibrary1
{
    using System;

    [Flags]
    public enum PrimeGeneratorOptions
    {
        None = 0b_0,
        ThrowOnCancel = 0b_1
    }
}
