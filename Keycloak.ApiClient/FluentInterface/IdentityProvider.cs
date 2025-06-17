using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class IdentityProvider
    {
        public string Alias => Representation?.Alias ?? string.Empty;
        public IdentityProviderRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public IdentityProvider(
            Realm realm
            )
        {

        }
    }
    public static partial class RealmExtensions
    {
        public async static Task<ICollection<IdentityProvider>> GetAllIdentityProvidersAsync(this Realm realm, bool? briefRepresentation = null, int? first = null, int? max = null, bool? realmOnly = null, string search = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesGetAsync(realm.Name, briefRepresentation, first, max, realmOnly, search);
            var result = data.Result.Select(x => realm.GetIdentityProviderObject(x)).ToList();
            return result;
        }

        public async static Task<IdentityProvider> GetIdentityProviderAsync(this Realm realm, string alias)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesGetAsync(realm: realm.Name, alias: alias);
            var result = realm.GetIdentityProviderObject(data.Result);
            return result;
        }

        public async static Task<IdentityProvider> CreateIdentityProviderAsync(this Realm realm, IdentityProviderRepresentation representation)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesPostAsync(realm.Name, representation);
            var result = realm.GetIdentityProviderObject(representation);
            return result;
        }

        private static IdentityProvider GetIdentityProviderObject(this Realm realm, IdentityProviderRepresentation representation)
        {
            var result = new IdentityProvider(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class IdentityProviderExtensions
    {
        public async static Task<IdentityProvider> UpdateAsync(this IdentityProvider obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesPutAsync(realm: obj.Realm.Name, alias: obj.Representation.Alias, obj.Representation);
            return obj;
        }

        public async static Task<IdentityProvider> DeleteAsync(this IdentityProvider obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesDeleteAsync(realm: obj.Realm.Name, alias: obj.Representation.Alias);
            return obj;
        }
    }
}
