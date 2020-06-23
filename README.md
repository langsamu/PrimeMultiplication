# Prime Multiplication

An application that generates multiplication tables of prime numbers.

The prime number generator is intentionally simple (increment, use unoptimised trial division for primality test).

But it's built using asynchronous streams, which means it works well on the web and on the command line with zero memory footprint and no buffering.

My focus is to demo an actual service, not to improve the sieve of Eratosthenes.

[![Codecov](https://codecov.io/gh/langsamu/PrimeMultiplication/branch/master/graph/badge.svg)](https://codecov.io/gh/langsamu/PrimeMultiplication)

[![Build Status](https://dev.azure.com/langsamu/PrimeMultiplication/_apis/build/status/Build?branchName=master)](https://dev.azure.com/langsamu/PrimeMultiplication/_build?definitionId=21)

## Online components
- [Web UI](https://prime-multiplication.azurewebsites.net/) (on Azure App Service)
- [Web API](https://prime-multiplication.azurewebsites.net/openapi) (Swagger UI)
- [Continuous deployment](https://dev.azure.com/langsamu/PrimeMultiplication/_build?definitionId=21) (Azure DevOps)
- [Infrastructure](https://portal.azure.com/) (Azure, requires login)
- [Telemetry for UI, API and CLI](https://portal.azure.com/#resource/subscriptions/d40c53cc-9981-4d98-a471-35df02d0bdc7/resourceGroups/PrimeMultiplication/providers/microsoft.insights/components/PrimeMultiplication/searchV1) (Application Insights, requires login)

## Source-code components

- [Class library](./PrimeMultiplication) (see for algorithm)
- [Command-line interface](./PrimeMultiplication.Cli)
- [Web interface](./PrimeMultiplication.Web)
- [Tests](./PrimeMultiplication.Tests)
- [Availability monitoring](./PrimeMultiplication.Availability)
- [Build pipeline](https://github.com/langsamu/PrimeMultiplication/blob/master/azure-pipelines.yml)

### Notable commits

1. [Proof of concept](https://github.com/langsamu/PrimeMultiplication/commit/206aca651699cde0391248907862bacfe629facd)
2. [Robust prime generator using asynchronous streams](https://github.com/langsamu/PrimeMultiplication/commit/849d1e044a8c668bde5df64a7cad3d12b752d9ae)
3. [Multiplication table as first class citizen](https://github.com/langsamu/PrimeMultiplication/commit/849d1e044a8c668bde5df64a7cad3d12b752d9ae)
4. [Azure DevOps build pipeline and Codecov integration](https://github.com/langsamu/PrimeMultiplication/commit/10492d7768c0d38ffc4ca4740aa16f26e5eb60d0)
5. [Curiosity one-liner attempt using async LINQ](https://github.com/langsamu/PrimeMultiplication/commit/276ca4a3553de2756cf55ac2e5de9701ba5e264d#diff-a485bc2848c9972199775734a926e50fR55-R81)
6. [Web API supporting JSON, XML and CSV](https://github.com/langsamu/PrimeMultiplication/commit/5a29187347a8b463673eaa08f96cc706eae77b59)
7. [Robust test-suite](https://github.com/langsamu/PrimeMultiplication/compare/5a29187347a8b463673eaa08f96cc706eae77b59...1829e071ac3a55038900d5c0a688274c5edbada5) achieves [100% test coverage](https://codecov.io/gh/langsamu/PrimeMultiplication/commit/1829e071ac3a55038900d5c0a688274c5edbada5)
7. [Continuous deployment to Azure App Service](https://github.com/langsamu/PrimeMultiplication/commit/3bc7ede1218ca77e60c76e2be56658ccd12a9a94)
9. [CLI telemetry](https://github.com/langsamu/PrimeMultiplication/commit/a644cba37b9566532961a27a0fd03086a2b03145)

### Bonus content

Sieve of Eratosthenes algorithm as an abstract syntax tree in an RDF graph.

- View as [XML](https://github.com/langsamu/GraphEngine/blob/4b301753f40986edb0c6af5259c159033f0b4248/GraphEngine.Tests/Resources/Examples/SieveOfEratosthenes.xml) (XML/RDF)
- View as [JSON](https://github.com/langsamu/GraphEngine/blob/4b301753f40986edb0c6af5259c159033f0b4248/GraphEngine.Tests/Resources/Examples/SieveOfEratosthenes.json) (JSON-LD)
- View as [Turtle](https://github.com/langsamu/GraphEngine/blob/4b301753f40986edb0c6af5259c159033f0b4248/GraphEngine.Tests/Resources/Examples/SieveOfEratosthenes.ttl)

Part of my ExpressionRDF side-project, which translates these graphs into [LINQ Expressions](https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression) that can be comiled and executed.
