using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class ClientScope
    {
        public string Name => Representation?.Name ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public ClientScopeRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public ClientScope(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<ClientScope>> GetAllClientScopesAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientScopesGetAsync(realm.Name);
            var result = data.Result.Select(x => realm.GetClientScopeObject(x)).ToList();
            return result;
        }

        public async static Task<ClientScope> GetClientScopeAsync(this Realm realm, string client_scope_id)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientScopesGetAsync(realm.Name, client_scope_id);
            var result = realm.GetClientScopeObject(data.Result);
            return result;
        }

        public async static Task<ICollection<ClientScope>> GetAllClientTemplatesAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientTemplatesGetAsync(realm.Name);
            var result = data.Result.Select(x => realm.GetClientScopeObject(x)).ToList();
            return result;
        }

        public async static Task<ClientScope> GetClientTemplatesAsync(this Realm realm, string client_scope_id)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientTemplatesGetAsync(realm.Name, client_scope_id);
            var result = realm.GetClientScopeObject(data.Result);
            return result;
        }

        public async static Task<ICollection<ClientScope>> GetAllDefaultClientScopesAsync(this Realm realm, string client_uuid)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientsDefaultClientScopesGetAsync(realm.Name, client_uuid);
            var result = data.Result.Select(x => realm.GetClientScopeObject(x)).ToList();
            return result;
        }

        public async static Task<ICollection<ClientScope>> GetAllOptionalClientScopesAsync(this Realm realm, string client_uuid)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientsOptionalClientScopesGetAsync(realm.Name, client_uuid);
            var result = data.Result.Select(x => realm.GetClientScopeObject(x)).ToList();
            return result;
        }

        public async static Task<ICollection<ClientScope>> GetAllDefaultDefaultClientScopesAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsDefaultDefaultClientScopesGetAsync(realm.Name);
            var result = data.Result.Select(x => realm.GetClientScopeObject(x)).ToList();
            return result;
        }

        public async static Task<ICollection<ClientScope>> GetAllDefaultOptionalClientScopesAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsDefaultOptionalClientScopesGetAsync(realm.Name);
            var result = data.Result.Select(x => realm.GetClientScopeObject(x)).ToList();
            return result;
        }

        private static ClientScope GetClientScopeObject(this Realm realm, ClientScopeRepresentation representation)
        {
            var result = new ClientScope(realm)
            {
                Representation = representation
            };
            return result;
        }
    }
}