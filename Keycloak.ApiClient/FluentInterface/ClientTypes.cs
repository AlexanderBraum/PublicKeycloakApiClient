
using keycloak;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class ClientTypes
    {
        public ClientTypesRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public ClientTypes(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ClientTypes> GetClientTypesAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientTypesGetAsync(realm.Name);
            var result = realm.GetClientTypesObject(data.Result);
            return result;
        }

        private static ClientTypes GetClientTypesObject(this Realm realm, ClientTypesRepresentation representation)
        {
            var result = new ClientTypes(realm)
            {
                Representation = representation
            };
            return result;
        }
    }
}