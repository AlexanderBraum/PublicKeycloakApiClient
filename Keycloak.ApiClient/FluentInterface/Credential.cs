using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class Credential
    {
        public string Id => Representation?.Id ?? string.Empty;
        public CredentialRepresentation Representation { get; set; }
        public Client Client { get; set; }

        public Credential(
            Client client
            )
        {
            Client = client;
        }
    }

    public class UserCredential
    {
        public string Id => Representation?.Id ?? string.Empty;
        public CredentialRepresentation Representation { get; set; }
        public RealmUser User { get; set; }

        public UserCredential(
            RealmUser user
            )
        {
            User = user;
        }
    }

    public static partial class ClientExtensions
    {
        public async static Task<Credential> GetClientSecret(this Client client)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsClientSecretGetAsync(
                realm: client.Realm.Name,
                client_uuid: client.Id);
            var result = client.GetCredentialObject(data.Result);
            return result;
        }

        public async static Task<Credential> GenerateNewSecretForTheClient(this Client client)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsClientSecretPostAsync(
                realm: client.Realm.Name,
                client_uuid: client.Id);
            var result = client.GetCredentialObject(data.Result);
            return result;
        }

        public async static Task<Credential> GetRotatedClientSecret(this Client client)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsClientSecretRotatedGetAsync(
                realm: client.Realm.Name,
                client_uuid: client.Id);
            var result = client.GetCredentialObject(data.Result);
            return result;
        }

        private static Credential GetCredentialObject(this Client client, CredentialRepresentation representation)
        {
            var result = new Credential(client)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class UserExtensions
    {
        public async static Task<ICollection<UserCredential>> GetCredentials(this RealmUser user)
        {
            var data = await user.Realm.Client.GeneratedClient.AdminRealmsUsersCredentialsGetAsync(
                realm: user.Realm.Name,
                user_id: user.Id);
            var result = data.Result.Select(x => user.GetUserCredentialObject(x)).ToList();
            return result;
        }

        private static UserCredential GetUserCredentialObject(this RealmUser user, CredentialRepresentation representation)
        {
            var result = new UserCredential(user)
            {
                Representation = representation
            };
            return result;
        }
    }
}