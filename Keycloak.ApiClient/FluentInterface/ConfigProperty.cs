using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class ConfigProperty
    {
        public string Name => Representation?.Name ?? string.Empty;
        public ConfigPropertyRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public ConfigProperty(
            Realm realm
            )
        {
            Realm = realm;
        }
    }
    public static partial class RealmExtensions
    {
        public async static Task<IEnumerable<(string, IEnumerable<ConfigProperty>)>> GetAllAuthenticationPerClientConfigDescriptionsAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAuthenticationPerClientConfigDescriptionAsync(realm.Name);
            var result = data.Result.Select(x => (x.Key, x.Value.Select(y => realm.GetConfigPropertyObject(y))));
            return result;
        }

        private static ConfigProperty GetConfigPropertyObject(this Realm realm, ConfigPropertyRepresentation representation)
        {
            var result = new ConfigProperty(realm)
            {
                Representation = representation
            };
            return result;
        }
    }
}
