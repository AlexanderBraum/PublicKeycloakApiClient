
using keycloak;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class AuthenticationExecution
    {
        public string Id => Representation?.Id ?? string.Empty;
        public AuthenticationExecutionRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public AuthenticationExecution(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<AuthenticationExecution> GetAuthenticationExecutionAsync(this Realm realm, string executionId)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAuthenticationExecutionsGetAsync(executionId: executionId, realm: realm.Name);
            var result = realm.GetAuthenticationExecutionObject(data.Result);
            return result;
        }

        private static AuthenticationExecution GetAuthenticationExecutionObject(this Realm realm, AuthenticationExecutionRepresentation representation)
        {
            var result = new AuthenticationExecution(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class AuthenticationExecutionExtensions
    {
        public async static Task<AuthenticationExecution> UpdateAsync(this AuthenticationExecution authenticationExecution)
        {
            await authenticationExecution.Realm.Client.GeneratedClient.AdminRealmsAuthenticationExecutionsPostAsync(realm: authenticationExecution.Realm.Name, body: authenticationExecution.Representation);
            return authenticationExecution;
        }

        public async static Task<AuthenticationExecution> DeleteAsync(this AuthenticationExecution authenticationExecution)
        {
            await authenticationExecution.Realm.Client.GeneratedClient.AdminRealmsAuthenticationExecutionsDeleteAsync(executionId: authenticationExecution.Id, realm: authenticationExecution.Realm.Name);
            return authenticationExecution;
        }

        public async static Task<AuthenticationExecution> LowerPriorityAsync(this AuthenticationExecution authenticationExecution)
        {
            await authenticationExecution.Realm.Client.GeneratedClient.AdminRealmsAuthenticationExecutionsLowerPriorityAsync(executionId: authenticationExecution.Id, realm: authenticationExecution.Realm.Name);
            return authenticationExecution;
        }

        public async static Task<AuthenticationExecution> RaisePriorityAsync(this AuthenticationExecution authenticationExecution)
        {
            await authenticationExecution.Realm.Client.GeneratedClient.AdminRealmsAuthenticationExecutionsRaisePriorityAsync(executionId: authenticationExecution.Id, realm: authenticationExecution.Realm.Name);
            return authenticationExecution;
        }
    }
}
