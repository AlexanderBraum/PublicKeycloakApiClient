using keycloak;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class AbstractPolicy
    {
        public string Id => Representation?.Id ?? string.Empty;
        public string Name => Representation?.Name ?? string.Empty;
        public AbstractPolicyRepresentation Representation { get; set; }
        public Client Client { get; set; }

        public AbstractPolicy(Client client)
        {
            Client = client;
        }
    }

    public static partial class ClientExtensions
    {
        public async static Task<ICollection<AbstractPolicy>> GetAllServerPolicysAsync(this Client client, string fields = null, int? first = null, int? max = null, string name = null, string owner = null, bool? permission = null, string policyId = null, string resource = null, string resourceType = null, string scope = null, string type = null)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPolicyGetAsync(realm: client.Realm.Name, client_uuid: client.Id, fields: fields, first: first, max: max, name: name, owner: owner, permission: permission, policyId: policyId, resource: resource, resourceType: resourceType, scope: scope, type: type);
            var result = data.Result.Select(x => client.GetComponentObject(x)).ToList();
            return result;
        }

        public async static Task<AbstractPolicy> SearchServerPolicyAsync(this Client client, string fields = null, string name = null)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPolicySearchAsync(realm: client.Realm.Name, client_uuid: client.Id, fields: fields, name: name);
            var result = client.GetComponentObject(data.Result);
            return result;
        }

        public async static Task<AbstractPolicy> CreateServerPolicyAsync(this Client client, AbstractPolicyRepresentation representation)
        {
            var json = JsonConvert.SerializeObject(representation);
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPolicyPostAsync(realm: client.Realm.Name, client_uuid: client.Id, body: json);
            var result = client.GetComponentObject(representation);
            return result;
        }

        public async static Task<ICollection<AbstractPolicy>> GetAllServerPermissionsAsync(this Client client, string fields = null, int? first = null, int? max = null, string name = null, string owner = null, bool? permission = null, string policyId = null, string resource = null, string resourceType = null, string scope = null, string type = null)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPermissionGetAsync(realm: client.Realm.Name, client_uuid: client.Id, fields: fields, first: first, max: max, name: name, owner: owner, permission: permission, policyId: policyId, resource: resource, resourceType: resourceType, scope: scope, type: type);
            var result = data.Result.Select(x => client.GetComponentObject(x)).ToList();
            return result;
        }

        public async static Task<AbstractPolicy> SearchServerPermissionAsync(this Client client, string fields = null, string name = null)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPermissionSearchAsync(realm: client.Realm.Name, client_uuid: client.Id, fields: fields, name: name);
            var result = client.GetComponentObject(data.Result);
            return result;
        }

        public async static Task<AbstractPolicy> CreateServerPermissionAsync(this Client client, AbstractPolicyRepresentation representation)
        {
            var json = JsonConvert.SerializeObject(representation);
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPermissionPostAsync(realm: client.Realm.Name, client_uuid: client.Id, body: json);
            var result = client.GetComponentObject(representation);
            return result;
        }

        private static AbstractPolicy GetComponentObject(this Client client, AbstractPolicyRepresentation representation)
        {
            var result = new AbstractPolicy(client)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static class AbstractPolicyExtensions
    {
        public async static Task<AbstractPolicy> UpdatePolicyAsync(this AbstractPolicy obj)
        {
            var json = JsonConvert.SerializeObject(obj.Representation);
            await obj.Client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPolicyPostAsync(realm: obj.Client.Realm.Name, client_uuid: obj.Client.Id, body: json);
            return obj;
        }

        public async static Task<AbstractPolicy> UpdatePermissionAsync(this AbstractPolicy obj)
        {
            var json = JsonConvert.SerializeObject(obj.Representation);
            await obj.Client.Realm.Client.GeneratedClient.AdminRealmsClientsAuthzResourceServerPermissionPostAsync(realm: obj.Client.Realm.Name, client_uuid: obj.Client.Id, body: json);
            return obj;
        }
    }
}