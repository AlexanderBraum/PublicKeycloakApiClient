//AdminRealmsClientScopesScopeMappingsClientsGetAsync
//AdminRealmsClientScopesScopeMappingsClientsPostAsync
//AdminRealmsClientScopesScopeMappingsClientsDeleteAsync
//AdminRealmsClientScopesScopeMappingsClientsAvailableAsync
//AdminRealmsClientScopesScopeMappingsClientsCompositeAsync
//AdminRealmsClientScopesScopeMappingsRealmGetAsync
//AdminRealmsClientScopesScopeMappingsRealmPostAsync
//AdminRealmsClientScopesScopeMappingsRealmDeleteAsync
//AdminRealmsClientScopesScopeMappingsRealmAvailableAsync
//AdminRealmsClientScopesScopeMappingsRealmCompositeAsync
//AdminRealmsClientTemplatesScopeMappingsClientsGetAsync
//AdminRealmsClientTemplatesScopeMappingsClientsPostAsync
//AdminRealmsClientTemplatesScopeMappingsClientsDeleteAsync
//AdminRealmsClientTemplatesScopeMappingsClientsAvailableAsync
//AdminRealmsClientTemplatesScopeMappingsClientsCompositeAsync
//AdminRealmsClientTemplatesScopeMappingsRealmGetAsync
//AdminRealmsClientTemplatesScopeMappingsRealmPostAsync
//AdminRealmsClientTemplatesScopeMappingsRealmDeleteAsync
//AdminRealmsClientTemplatesScopeMappingsRealmAvailableAsync
//AdminRealmsClientTemplatesScopeMappingsRealmCompositeAsync
//AdminRealmsClientsEvaluateScopesScopeMappingsGrantedAsync
//AdminRealmsClientsEvaluateScopesScopeMappingsNotGrantedAsync
//AdminRealmsClientsRolesCompositesGetAsync
//AdminRealmsClientsRolesCompositesPostAsync
//AdminRealmsClientsRolesCompositesDeleteAsync
//AdminRealmsClientsRolesCompositesClientsAsync
//AdminRealmsClientsRolesCompositesRealmAsync
//AdminRealmsClientsScopeMappingsClientsGetAsync
//AdminRealmsClientsScopeMappingsClientsPostAsync
//AdminRealmsClientsScopeMappingsClientsDeleteAsync
//AdminRealmsClientsScopeMappingsClientsAvailableAsync
//AdminRealmsClientsScopeMappingsClientsCompositeAsync
//AdminRealmsClientsScopeMappingsRealmGetAsync
//AdminRealmsClientsScopeMappingsRealmPostAsync
//AdminRealmsClientsScopeMappingsRealmDeleteAsync
//AdminRealmsClientsScopeMappingsRealmAvailableAsync
//AdminRealmsClientsScopeMappingsRealmCompositeAsync
//AdminRealmsGroupsRoleMappingsClientsGetAsync
//AdminRealmsGroupsRoleMappingsClientsPostAsync
//AdminRealmsGroupsRoleMappingsClientsDeleteAsync
//AdminRealmsGroupsRoleMappingsClientsAvailableAsync
//AdminRealmsGroupsRoleMappingsClientsCompositeAsync
//AdminRealmsGroupsRoleMappingsRealmGetAsync
//AdminRealmsGroupsRoleMappingsRealmPostAsync
//AdminRealmsGroupsRoleMappingsRealmDeleteAsync
//AdminRealmsGroupsRoleMappingsRealmAvailableAsync
//AdminRealmsGroupsRoleMappingsRealmCompositeAsync
//AdminRealmsRolesByIdCompositesGetAsync
//AdminRealmsRolesByIdCompositesPostAsync
//AdminRealmsRolesByIdCompositesDeleteAsync
//AdminRealmsRolesByIdCompositesClientsAsync
//AdminRealmsRolesByIdCompositesRealmAsync
//AdminRealmsRolesCompositesGetAsync
//AdminRealmsRolesCompositesPostAsync
//AdminRealmsRolesCompositesDeleteAsync
//AdminRealmsRolesCompositesClientsAsync
//AdminRealmsRolesCompositesRealmAsync
//AdminRealmsUsersRoleMappingsClientsGetAsync
//AdminRealmsUsersRoleMappingsClientsPostAsync
//AdminRealmsUsersRoleMappingsClientsDeleteAsync
//AdminRealmsUsersRoleMappingsClientsAvailableAsync
//AdminRealmsUsersRoleMappingsClientsCompositeAsync
//AdminRealmsUsersRoleMappingsRealmGetAsync
//AdminRealmsUsersRoleMappingsRealmPostAsync
//AdminRealmsUsersRoleMappingsRealmDeleteAsync
//AdminRealmsUsersRoleMappingsRealmAvailableAsync
//AdminRealmsUsersRoleMappingsRealmCompositeAsync

using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public interface IRole
    {
        public string Name { get; }
        public string Id { get; }
        public RoleRepresentation Representation { get; }
    }

    public class RealmRole : IRole
    {
        public string Name => Representation?.Name ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public RoleRepresentation Representation { get; set; }
        public Realm Realm { get; set; }
        public RealmRole(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<IRole>> GetAllRolesAsync(
            this Realm realm,
            bool? briefRepresentation = null,
            int? first = null,
            int? max = null,
            string search = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsRolesGetAsync(
                realm: realm.Name,
                briefRepresentation: briefRepresentation,
                first: first,
                max: max,
                search: search);
            var result = data.Result.Select(x => realm.GetRoleObject(x)).ToList();
            return result;
        }

        public async static Task<IRole> GetRoleAsync(this Realm realm, string role_id)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsRolesByIdGetAsync(
                realm: realm.Name,
                role_id: role_id);
            var result = realm.GetRoleObject(data.Result);
            return result;
        }

        public async static Task<IRole> CreateRoleAsync(this Realm realm, RoleRepresentation representation)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsRolesPostAsync(realm.Name, representation);
            var result = realm.GetRoleObject(representation);
            return result;
        }

        private static IRole GetRoleObject(this Realm realm, RoleRepresentation representation)
        {
            var result = new RealmRole(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class IRoleExtensions
    {
        public async static Task<IRole> UpdateAsync(this RealmRole obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsRolesByIdPutAsync(obj.Realm.Name, obj.Id, obj.Representation);
            return obj;
        }

        public async static Task<IRole> DeleteAsync(this RealmRole obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsRolesByIdDeleteAsync(obj.Realm.Name, obj.Id);
            return obj;
        }
    }

    public class ClientRole : IRole
    {
        public string Name => Representation?.Name ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public RoleRepresentation Representation { get; set; }
        public Client Client { get; set; }
        public ClientRole(
            Client client
            )
        {
            Client = client;
        }
    }

    public static partial class ClientExtensions
    {
        public async static Task<ICollection<IRole>> GetAllClientRolesAsync(
            this Client client,
            bool? briefRepresentation = null,
            int? first = null,
            int? max = null,
            string search = null)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsRolesGetAsync(
                realm: client.Realm.Name,
                client_uuid: client.Id,
                briefRepresentation: briefRepresentation,
                first: first,
                max: max,
                search: search);
            var result = data.Result.Select(x => client.GetClientRoleObject(x)).ToList();
            return result;
        }

        public async static Task<IRole> CreateClientRoleAsync(this Client client, RoleRepresentation representation)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsRolesPostAsync(
                client.Realm.Name,
                client.Id,
                representation);
            var result = client.GetClientRoleObject(representation);
            return result;
        }
        private static IRole GetClientRoleObject(this Client client, RoleRepresentation representation)
        {
            var result = new ClientRole(client)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ClientRoleExtensions
    {
        public async static Task<IRole> UpdateAsync(this ClientRole obj)
        {
            await obj.Client.Realm.Client.GeneratedClient.AdminRealmsClientsRolesPutAsync(
                role_name: obj.Name,
                realm: obj.Client.Realm.Name,
                client_uuid: obj.Client.Id,
                obj.Representation);
            return obj;
        }
        public async static Task<IRole> DeleteAsync(this ClientRole obj)
        {
            await obj.Client.Realm.Client.GeneratedClient.AdminRealmsClientsRolesDeleteAsync(
                role_name: obj.Name,
                realm: obj.Client.Realm.Name,
                client_uuid: obj.Client.Id);
            return obj;
        }
    }
}