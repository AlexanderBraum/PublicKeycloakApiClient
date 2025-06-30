using keycloak;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class ClientProfiles
    {
        public ClientProfilesRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public ClientProfiles(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ClientProfiles> GetClientProfilesAsync(this Realm realm, bool? include_global_profiles = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientPoliciesProfilesGetAsync(realm.Name, include_global_profiles);
            var result = realm.GetClientProfilesObject(data.Result);
            return result;
        }

        private static ClientProfiles GetClientProfilesObject(this Realm realm, ClientProfilesRepresentation representation)
        {
            var result = new ClientProfiles(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ClientProfilesExtensions
    {
        public async static Task<ClientProfiles> UpdateAsync(this ClientProfiles obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsClientPoliciesProfilesPutAsync(obj.Realm.Name, obj.Representation);
            return obj;
        }
    }
}