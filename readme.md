# Keycloak.ApiClient
This is a simple ApiClient for Keycloak, generated with NSwagStudio using the OpenAPI specification from Keycloak ([OpenAPI JSON](https://www.keycloak.org/docs-api/latest/rest-api/openapi.json)).

## Getting Started Generated ApiClient
```csharp

var client = await KeycloakApiClientFactory.GetKeycloakApiClientAsync("https://MyKeycloakInst.com", "username", "password");
var realms = await client.AdminRealmsGetAsync();

```

## Getting Started Fluent ApiClient (Wrapper for Generated ApiClient)
```csharp

var client = await KeycloakApiClientFactory.GetFluentKeycloakApiClientAsync("https://MyKeycloakInst.com", "username", "password");
var realm = await client.GetRealmAsync("RealmName");
var client = await realm.CreateClientAsync(
            new ClientRepresentation
            {
                Name = "ClientName",
                Id = "ClientId"
            });
```

## Features
- Compatible with .NET Standard 2.1
- Implements all endpoints from the Keycloak OpenAPI specification as methods in a single ApiClient class
- Adds a (limited) Fluent API client wrapper for writing declarative code.

## Requirements
- Keycloak v17+ (or any version supporting the [OpenAPI specification](https://www.keycloak.org/docs-api/latest/rest-api/openapi.json))
- .NET Standard 2.1 compatible.

## Notes
- The client is auto-generated; for advanced usage or custom endpoints, consider extending the generated code.
- This is a simple ApiClient for Keycloak. It's generated with NSwagStudio using the [OpenAPI specification from Keycloak](https://www.keycloak.org/docs-api/latest/rest-api/openapi.json).
- There is a fluent wrapper around the generated ApiClient, which allows you to write more declarative code. This is not a full fluent wrapper.
- The Fluent ApiClient is updated periodically to keep up with demand. If you need a specific feature, please open an issue or pull request.

## Source code
- [GitHub repo: Keycloak.ApiClient](https://github.com/AlexanderBraum/PublicKeycloakApiClient)

# Project files:
- KeycloakApiClientFactory.cs: Factory class to create an instance of the KeycloakApiClient with authentication.
- KeycloakApiClient.cs: The main ApiClient class, generated from the OpenAPI specification with NSwagStudio.
- source/nswag.nswag: NSwag configuration file for generating the ApiClient.
- source/keycloak__openapi.json: Copy of the OpenAPI specification file for Keycloak, used to generate the ApiClient.
- FluentInterface/*: Contains the Fluent ApiClient wrapper classes and methods.
- FluentInterface/Core/*: Contains the shared helper classes for implementing the Fluent ApiClient.