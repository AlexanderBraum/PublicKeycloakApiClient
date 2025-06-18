using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class IdentityProviderMapper
    {
        public string Id => Representation?.Id ?? string.Empty;
        public string Name => Representation?.Name ?? string.Empty;
        public IdentityProviderMapperRepresentation Representation { get; set; }
        public IdentityProvider IdentityProvider { get; set; }

        public IdentityProviderMapper(
            IdentityProvider identityProvider
            )
        {
            IdentityProvider = identityProvider;
        }
    }

    public static partial class IdentityProviderExtensions
    {
        public async static Task<ICollection<IdentityProviderMapper>> GetAllIdentityProviderMappersAsync(this IdentityProvider identityProvider, string realm, string alias)
        {
            var data = await identityProvider.Realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesMappersGetAsync(identityProvider.Realm.Name, identityProvider.Alias);
            var result = data.Result.Select(x => identityProvider.GetIdentityProviderMapperObject(x)).ToList();
            return result;
        }

        public async static Task<IdentityProviderMapper> GetIdentityProviderMapperAsync(this IdentityProvider identityProvider, string id)
        {
            var data = await identityProvider.Realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesMappersGetAsync(realm: identityProvider.Realm.Name, alias: identityProvider.Alias, id: id);
            var result = identityProvider.GetIdentityProviderMapperObject(data.Result);
            return result;
        }

        public async static Task<IdentityProviderMapper> CreateIdentityProviderMapperAsync(this IdentityProvider identityProvider, IdentityProviderMapperRepresentation representation)
        {
            var data = await identityProvider.Realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesMappersPostAsync(identityProvider.Realm.Name, identityProvider.Alias, representation);
            var result = identityProvider.GetIdentityProviderMapperObject(representation);
            return result;
        }

        private static IdentityProviderMapper GetIdentityProviderMapperObject(this IdentityProvider identityProvider, IdentityProviderMapperRepresentation representation)
        {
            var result = new IdentityProviderMapper(identityProvider)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class IdentityProviderMapperExtensions
    {
        public async static Task<IdentityProviderMapper> UpdateAsync(this IdentityProviderMapper obj)
        {
            await obj.IdentityProvider.Realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesMappersPutAsync(id: obj.Id, realm: obj.IdentityProvider.Realm.Name, alias: obj.IdentityProvider.Alias, body: obj.Representation);
            return obj;
        }

        public async static Task<IdentityProviderMapper> DeleteAsync(this IdentityProviderMapper obj)
        {
            await obj.IdentityProvider.Realm.Client.GeneratedClient.AdminRealmsIdentityProviderInstancesMappersDeleteAsync(id: obj.Id, realm: obj.IdentityProvider.Realm.Name, alias: obj.IdentityProvider.Alias);
            return obj;
        }
    }
}
