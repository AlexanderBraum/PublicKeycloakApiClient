using keycloak;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public interface IUser
    {
        public string Id { get; }
        public UserRepresentation Representation { get; }
    }

    public class RealmUser : IUser
    {
        public string Id => Representation?.Id ?? string.Empty;
        public UserRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public RealmUser(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public class ClientUser : IUser
    {
        public string Id => Representation?.Id ?? string.Empty;
        public UserRepresentation Representation { get; set; }
        public Client Client { get; set; }

        public ClientUser(
            Client client
            )
        {
            Client = client;
        }
    }

    public class GroupUser : IUser
    {
        public string Id => Representation?.Id ?? string.Empty;
        public UserRepresentation Representation { get; set; }
        public Group Group { get; set; }

        public GroupUser(
            Group group
            )
        {
            Group = group;
        }
    }

    public class RoleUser : IUser
    {
        public string Id => Representation?.Id ?? string.Empty;
        public UserRepresentation Representation { get; set; }
        public IRole Role { get; set; }

        public RoleUser(
            IRole role
            )
        {
            Role = role;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<IUser>> GetAllUsersAsync(
            this Realm realm,
            bool? briefRepresentation = null,
            string email = null,
            bool? emailVerified = null,
            bool? enabled = null,
            bool? exact = null,
            int? first = null,
            string firstName = null,
            string idpAlias = null,
            string idpUserId = null,
            string lastName = null,
            int? max = null,
            string q = null,
            string search = null,
            string username = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsUsersGetAsync(
                realm.Name,
                briefRepresentation,
                email,
                emailVerified,
                enabled,
                exact,
                first,
                firstName,
                idpAlias,
                idpUserId,
                lastName,
                max,
                q,
                search,
                username);
            var result = data.Result.Select(x => realm.GetUserObject(x)).ToList();
            return result;
        }

        public async static Task<IUser> GetUserAsync(this Realm realm, string user_id, bool? userProfileMetadata = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsUsersGetAsync(realm.Name, user_id, userProfileMetadata);
            var result = realm.GetUserObject(data.Result);
            return result;
        }

        public async static Task<ICollection<IUser>> GetUsersWithRoleAsync(
            this Realm realm,
            string role_name,
            bool? briefRepresentation = null,
            int? first = null,
            int? max = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsRolesUsersAsync(
                role_name: role_name,
                realm: realm.Name,
                briefRepresentation: briefRepresentation,
                first: first,
                max: max);
            var result = data.Result.Select(x => realm.GetUserObject(x)).ToList();
            return result;
        }

        private static IUser GetUserObject(this Realm realm, UserRepresentation representation)
        {
            var result = new RealmUser(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ClientExtensions
    {
        public async static Task<IUser> GetServiceAccountUserAsync(this Client client)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsServiceAccountUserAsync(
                realm: client.Realm.Name,
                client_uuid: client.Id);
            var result = client.GetClientUserObject(data.Result);
            return result;
        }
        private static IUser GetClientUserObject(this Client client, UserRepresentation representation)
        {
            var result = new ClientUser(client)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ClientRoleExtensions
    {
        public async static Task<ICollection<IUser>> GetUsersAsync(this ClientRole role, bool? briefRepresentation = null, int? first = null, int? max = null)
        {
            var data = await role.Client.Realm.Client.GeneratedClient.AdminRealmsClientsRolesUsersAsync(
                role_name: role.Name,
                realm: role.Client.Realm.Name,
                client_uuid: role.Client.Id,
                briefRepresentation: briefRepresentation,
                first: first,
                max: max);
            var result = data.Result.Select(x => role.GetRoleUserObject(x)).ToList();
            return result;
        }

        public async static Task<ICollection<IUser>> GetUsersGroupAsync(this ClientRole role, bool? briefRepresentation = null, int? first = null, int? max = null)
        {
            var data = await role.Client.Realm.Client.GeneratedClient.AdminRealmsClientsRolesGroupsAsync(
                role_name: role.Name,
                realm: role.Client.Realm.Name,
                client_uuid: role.Client.Id,
                briefRepresentation: briefRepresentation,
                first: first,
                max: max);
            var result = data.Result.Select(x => role.GetRoleUserObject(x)).ToList();
            return result;
        }

        private static IUser GetRoleUserObject(this IRole role, UserRepresentation representation)
        {
            var result = new RoleUser(role)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class GroupExtensions
    {
        public async static Task<ICollection<IUser>> GetUsersAsync(
            this Group group,
            bool? briefRepresentation = null,
            int? first = null,
            int? max = null)
        {
            var data = await group.Realm.Client.GeneratedClient.AdminRealmsGroupsMembersAsync(
                realm: group.Realm.Name,
                group_id: group.Id,
                briefRepresentation: briefRepresentation,
                first: first,
                max: max);
            var result = data.Result.Select(x => group.GetGroupUserObject(x)).ToList();
            return result;
        }

        public async static Task<ICollection<IUser>> GetUsersWithRoleNameAsync(
            this Group group,
            string role_name,
            bool? briefRepresentation = null,
            int? first = null,
            int? max = null)
        {
            var data = await group.Realm.Client.GeneratedClient.AdminRealmsRolesGroupsAsync(
                role_name: role_name,
                realm: group.Realm.Name,
                briefRepresentation: briefRepresentation,
                first: first,
                max: max);
            var result = data.Result.Select(x => group.GetGroupUserObject(x)).ToList();
            return result;
        }

        private static IUser GetGroupUserObject(this Group group, UserRepresentation representation)
        {
            var result = new GroupUser(group)
            {
                Representation = representation
            };
            return result;
        }
    }
}

