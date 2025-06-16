# Keycloak.ApiClient
This is a simple ApiClient for Keycloak, generated with NSwagStudio using the OpenAPI specification from Keycloak ([OpenAPI JSON](https://www.keycloak.org/docs-api/latest/rest-api/openapi.json)).

## Getting Started
```csharp

var client = await KeycloakApiClientFactory.GetKeycloakApiClientAsync("https://MyKeycloakInst.com", "username", "password");
var result = await client.AdminRealmsGetAsync();

```

## Features
- Compatible with .NET Standard 2.1
- Implements all endpoints from the Keycloak OpenAPI specification as methods in a single ApiClient class

## Requirements

- Keycloak v17+ (or any version supporting the [OpenAPI specification](https://www.keycloak.org/docs-api/latest/rest-api/openapi.json))
- .NET Standard 2.1 compadable.


## Notes
- The client is auto-generated; for advanced usage or custom endpoints, consider extending the generated code.
- This is a simple ApiClient for Keycloak. It's generated with NSwagStudio using the [OpenAPI specification from Keycloak](https://www.keycloak.org/docs-api/latest/rest-api/openapi.json).

## source code
- [GitHub repo: Keycloak.ApiClient](https://github.com/AlexanderBraum/PublicKeycloakApiClient)

# Project files:
- KeycloakApiClientFactory,cs: Factory class to create an instance of the KeycloakApiClient with authentication.
- KeycloakApiClient.cs: The main ApiClient class, generated from the OpenAPI specification with NSwagStudio.
- source/nswag.nswag: NSwag configuration file for generating the ApiClient.
- source/keycloak__openapi.json: Copy of the OpenAPI specification file for Keycloak, used to generate the ApiClient.