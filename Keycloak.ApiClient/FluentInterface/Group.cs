using keycloak;

namespace Keycloak.ApiClient.FluentInterface
{
    public class Group
    {
        public string Name => Representation?.Name ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public GroupRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public Group(
            Realm realm
            )
        {
            Realm = realm;
        }
    }
}