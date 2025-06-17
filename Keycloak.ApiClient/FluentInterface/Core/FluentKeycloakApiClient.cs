using keycloak;

namespace Keycloak.ApiClient.FluentInterface.Core
{
    public class FluentKeycloakApiClient
    {
        public KeycloakApiClient GeneratedClient { get; }

        public FluentKeycloakApiClient(KeycloakApiClient keycloakApiClient)
        {
            if (keycloakApiClient == null)
            {
                throw new KeycloakClientFluentInterfaceException("Generated client cannot be null.");
            }

            GeneratedClient = keycloakApiClient;
        }
    }
}
