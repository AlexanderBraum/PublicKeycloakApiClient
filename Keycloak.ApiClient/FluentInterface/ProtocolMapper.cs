using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class ProtocolMapper
    {
        public string Name => Representation?.Name ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public ProtocolMapperRepresentation Representation { get; set; }
        public Client Client { get; set; }

        public ProtocolMapper(
            Client client
            )
        {
            Client = client;
        }
    }

    public static partial class ClientExtensions
    {
        public async static Task<ICollection<ProtocolMapper>> GetAllProtocolMappersAsync(this Client client)
        {
            var data = await client.Realm.Client.AdminRealmsClientsProtocolMappersModelsGetAsync(client.Realm.Name, client.Id);
            var result = data.Result.Select(x => client.GetProtocolMapperObject(x)).ToList();
            return result;
        }

        public async static Task<ProtocolMapper> GetProtocolMapperAsync(this Client client, string id)
        {
            var data = await client.Realm.Client.AdminRealmsClientsProtocolMappersModelsGetAsync(realm: client.Realm.Name, client_uuid: client.Id, id: id);
            var result = client.GetProtocolMapperObject(data.Result);
            return result;
        }

        public async static Task<ProtocolMapper> CreateProtocolMapperAsync(this Client client, ProtocolMapperRepresentation representation)
        {
            var data = await client.Realm.Client.AdminRealmsClientsProtocolMappersModelsPostAsync(client.Realm.Name, client.Id, representation);
            var result = client.GetProtocolMapperObject(representation);
            return result;
        }

        private static ProtocolMapper GetProtocolMapperObject(this Client client, ProtocolMapperRepresentation representation)
        {
            var result = new ProtocolMapper(client)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ProtocolMapperExtensions
    {
        public async static Task<ProtocolMapper> UpdateAsync(this ProtocolMapper obj)
        {
            await obj.Client.Realm.Client.AdminRealmsClientsProtocolMappersModelsPutAsync(id: obj.Id, realm: obj.Client.Realm.Name, client_uuid: obj.Client.Id, obj.Representation);
            return obj;
        }

        public async static Task<ProtocolMapper> DeleteAsync(this ProtocolMapper obj)
        {
            await obj.Client.Realm.Client.AdminRealmsClientsProtocolMappersModelsDeleteAsync(id: obj.Id, realm: obj.Client.Realm.Name, client_uuid: obj.Client.Id);
            return obj;
        }
    }
}
