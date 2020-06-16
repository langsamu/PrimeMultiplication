# Command-line interface

This folder contains a .NET Core 3 console application project.

## How to run

Print help and usage:
```bat
PrimeMultiplication.Cli -?
```

Generate a multiplication table of **10 primes**:
```bat
PrimeMultiplication.Cli 10
```

Generate a multiplication table of **1000 primes** or as many as you can in **1 millisecond**:
```bat
PrimeMultiplication.Cli 1000 --timeout 1
```

Generate a multiplication table of **5000 primes** in **5 seconds** or die trying:
```bat
PrimeMultiplication.Cli 5000 --timeout 1 --throw-on-cancel
```

