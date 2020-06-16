# Web interface

This folder contains an ASP.NET Core 3 MVC project.

The web application is available at [prime-multiplication.azurewebsites.net](https://prime-multiplication.azurewebsites.net/).

It's deployed automatically by [this](https://dev.azure.com/langsamu/PrimeMultiplication/_build?definitionId=21) Azure DevOps account based on [this](../azure-pipelines.yml) YAML build pipeline.

## UI
The application exposes a [single page](./Controllers/DefaultController.cs) that renders the multiplication table as an HTML table (see it [live](https://prime-multiplication.azurewebsites.net/multiply/10))

## API
The application also exposes the same functionality over an [API](./Controllers/ApiController.cs#L29).

The API is described by a [machine-readable specification](./wwwroot/openapi.json) (see it [live](https://prime-multiplication.azurewebsites.net/openapi.json)).

The actual work of creating responses in various format is done by the [output formatters](./Formatters/TableFormatter.cs) ([JSON](./Formatters/JsonFormatter.cs), [XML](./Formatters/XmlFormatter.cs), [CSV](./Formatters/CsvFormatter.cs)).

The specification is rendered for documentation and testing purpose (try it [live](https://prime-multiplication.azurewebsites.net/openapi)).