
using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class Client
    {
        public string Name => Representation?.Name ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public ClientRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public Client(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<Client>> GetAllClientsAsync(this Realm realm, string clientId = null, int? first = null, int? max = null, string q = null, bool? search = null, bool? viewableOnly = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientsGetAsync(realm.Name, clientId, first, max, q, search, viewableOnly);
            var result = data.Result.Select(x => realm.GetClientObject(x)).ToList();
            return result;
        }

        public async static Task<Client> GetClientAsync(this Realm realm, string client_uuid)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientsGetAsync(realm.Name, client_uuid: client_uuid);
            var result = realm.GetClientObject(data.Result);
            return result;
        }

        public async static Task<Client> CreateClientAsync(this Realm realm, ClientRepresentation representation)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientsPostAsync(realm.Name, representation);
            var result = realm.GetClientObject(representation);
            return result;
        }

        private static Client GetClientObject(this Realm realm, ClientRepresentation representation)
        {
            var result = new Client(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ClientExtensions
    {
        public async static Task<Client> UpdateAsync(this Client obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsClientsPutAsync(obj.Realm.Name, obj.Id, obj.Representation);
            return obj;
        }

        public async static Task<Client> DeleteAsync(this Client obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsClientsDeleteAsync(obj.Realm.Name, obj.Id);
            return obj;
        }
    }
}
