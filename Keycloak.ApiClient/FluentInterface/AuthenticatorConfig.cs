
using keycloak;
using Keycloak.ApiClient.FluentInterface;
using System.Threading.Tasks;

namespace Keycloak.ApiAuthenticatorConfig.FluentInterface
{
    public class AuthenticatorConfig
    {
        public string Alias => Representation?.Alias ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public AuthenticatorConfigRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public AuthenticatorConfig(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<AuthenticatorConfig> GetAuthenticatorConfigAsync(this Realm realm, string id)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAuthenticationConfigGetAsync(id: id, realm: realm.Name);
            var result = realm.GetAuthenticatorConfigObject(data.Result);
            return result;
        }

        private static AuthenticatorConfig GetAuthenticatorConfigObject(this Realm realm, AuthenticatorConfigRepresentation representation)
        {
            var result = new AuthenticatorConfig(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class AuthenticatorConfigExtensions
    {
        public async static Task<AuthenticatorConfig> UpdateAsync(this AuthenticatorConfig obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsAuthenticationConfigPutAsync(obj.Realm.Name, obj.Id, obj.Representation);
            return obj;
        }

        public async static Task<AuthenticatorConfig> DeleteAsync(this AuthenticatorConfig obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsAuthenticationConfigDeleteAsync(obj.Realm.Name, obj.Id);
            return obj;
        }
    }
}
