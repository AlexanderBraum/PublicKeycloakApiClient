
using keycloak;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class AuthenticatorConfigInfo
    {
        public string Name => Representation?.Name ?? string.Empty;
        public AuthenticatorConfigInfoRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public AuthenticatorConfigInfo(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<AuthenticatorConfigInfo> GetAllAuthenticatorConfigInfosAsync(this Realm realm, string providerId)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAuthenticationConfigDescriptionAsync(providerId: providerId, realm: realm.Name);
            var result = realm.GetAuthenticatorConfigInfoObject(data.Result);
            return result;
        }

        private static AuthenticatorConfigInfo GetAuthenticatorConfigInfoObject(this Realm realm, AuthenticatorConfigInfoRepresentation representation)
        {
            var result = new AuthenticatorConfigInfo(realm)
            {
                Representation = representation
            };
            return result;
        }

    }

}
