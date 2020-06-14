# Availability Monitoring

This folder contains a WebTest project.

[Availability.webtest](https://github.com/langsamu/PrimeMultiplication/blob/master/PrimeMultiplication.Availability/Availability.webtest) is used for availability monitoring of the web application using Azure Application Insights.

## Monitoring results
Available [on the Azure portal](https://portal.azure.com/#resource/subscriptions/d40c53cc-9981-4d98-a471-35df02d0bdc7/resourceGroups/PrimeMultiplication/providers/microsoft.insights/components/PrimeMultiplication/availability) (requires login).

When site is unavailable, notification emails are sent to [this](https://www.mailinator.com/v3/index.jsp?query=avastprimemultiplication) (unmoderated Mailinator) inbox.

## Monitored requests
The test contains the following requests:
- API multiplies primes
- API can be cancelled
- API negotiates Accept header
- API negotiates 'file extension'
- UI multiplies primes
- UI can be cancelled
- UI can be throw when cancelled
