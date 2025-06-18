
using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class AuthenticationFlow
    {
        public string Alias => Representation?.Alias ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public AuthenticationFlowRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public AuthenticationFlow(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<AuthenticationFlow>> GetAllAuthenticationFlowsAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsGetAsync(realm.Name);
            var result = data.Result.Select(x => realm.GetAuthenticationFlowObject(x)).ToList();
            return result;
        }

        public async static Task<AuthenticationFlow> GetAuthenticationFlowAsync(this Realm realm, string id)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsGetAsync( id: id, realm.Name);
            var result = realm.GetAuthenticationFlowObject(data.Result);
            return result;
        }

        public async static Task<AuthenticationFlow> CreatetAuthenticationFlowAsync(this Realm realm, AuthenticationFlowRepresentation representation)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsPostAsync(realm.Name, representation);
            var result = realm.GetAuthenticationFlowObject(representation);
            return result;
        }

        private static AuthenticationFlow GetAuthenticationFlowObject(this Realm realm, AuthenticationFlowRepresentation representation)
        {
            var result = new AuthenticationFlow(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class AuthenticationFlowExtensions
    {
        public async static Task<AuthenticationFlow> CopyAsync(this AuthenticationFlow obj, string newName)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsCopyAsync(
                flowAlias: obj.Alias,
                realm: obj.Realm.Name, 
                body: new Dictionary<string, string> { { "newName", newName } });
            return obj;
        }

        public async static Task<AuthenticationFlow> UpdateAsync(this AuthenticationFlow obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsPutAsync(id: obj.Id, realm: obj.Realm.Name, body: obj.Representation);
            return obj;
        }

        public async static Task<AuthenticationFlow> DeleteAsync(this AuthenticationFlow obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsDeleteAsync(id: obj.Id, realm: obj.Realm.Name);
            return obj;
        }
    }
}
