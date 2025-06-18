
using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class AuthenticationExecutionInfo
    {
        public string Id => Representation?.Id ?? string.Empty;
        public AuthenticationExecutionInfoRepresentation Representation { get; set; }
        public AuthenticationFlow AuthenticationFlow { get; set; }

        public AuthenticationExecutionInfo(
            AuthenticationFlow authenticationFlow
            )
        {
            AuthenticationFlow = authenticationFlow;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<AuthenticationExecutionInfo>> GetAllAuthenticationExecutionInfoAsync(this AuthenticationFlow authenticationFlow)
        {
            var data = await authenticationFlow.Realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsExecutionsGetAsync(flowAlias: authenticationFlow.Alias, realm: authenticationFlow.Realm.Name);
            var result = data.Result.Select(x => authenticationFlow.GetAuthenticationExecutionInfoObject(x)).ToList();
            return result;
        }

        public async static Task<AuthenticationExecutionInfo> CreateAuthenticationExecutionInfoAsync(this AuthenticationFlow authenticationFlow, AuthenticationExecutionInfoRepresentation representation)
        {
            var data = await authenticationFlow.Realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsExecutionsPutAsync(flowAlias: authenticationFlow.Alias, realm: authenticationFlow.Realm.Name, representation);
            var result = authenticationFlow.GetAuthenticationExecutionInfoObject(representation);
            return result;
        }

        private static AuthenticationExecutionInfo GetAuthenticationExecutionInfoObject(this AuthenticationFlow authenticationFlow, AuthenticationExecutionInfoRepresentation representation)
        {
            var result = new AuthenticationExecutionInfo(authenticationFlow)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class AuthenticationExecutionInfoExtensions
    {
        public async static Task<AuthenticationExecutionInfo> UpdateAsync(this AuthenticationExecutionInfo obj)
        {
            await obj.AuthenticationFlow.Realm.Client.GeneratedClient.AdminRealmsAuthenticationFlowsExecutionsPutAsync(flowAlias: obj.AuthenticationFlow.Alias, realm: obj.AuthenticationFlow.Realm.Name, obj.Representation);
            return obj;
        }
    }
}
