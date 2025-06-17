using keycloak;
using Keycloak.ApiClient.FluentInterface.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class Realm
    {
        public string Name => Representation?.Realm ?? string.Empty;
        public KeycloakApiClient Client { get; }
        public RealmRepresentation Representation { get; set; }

        public Realm(
            KeycloakApiClient client)
        {
            Client = client;
        }
    }

    public static partial class KeycloakApiClientExtensions
    {
        public async static Task<ICollection<Realm>> GetAllRealmsAsync(this KeycloakApiClient client, bool? briefRepresentation = null)
        {
            var data = await client.AdminRealmsGetAsync(briefRepresentation);
            var result = data.Result.Select(x => client.GetRealmObject(x)).ToList();
            return result;
        }

        public async static Task<Realm> GetRealmAsync(this KeycloakApiClient client, string realm)
        {
            var data = await client.AdminRealmsGetAsync(realm);
            var result = client.GetRealmObject(data.Result);
            return result;
        }

        public async static Task<Realm> CreateRealmAsync(
            this KeycloakApiClient client,
            RealmRepresentation realmRepresentation)
        {
            var realm = client.GetRealmObject(realmRepresentation);
            realm = await realm.CreateAsync();
            return realm;
        }

        private static Realm GetRealmObject(this KeycloakApiClient client, RealmRepresentation realmRepresentation)
        {
            var result = new Realm(client)
            {
                Representation = realmRepresentation
            };
            return result;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<Realm> GetAsync(this Realm realm)
        {
            var result = await realm.Client.AdminRealmsGetAsync(realm.Name);
            realm.Representation = result.Result;
            return realm;
        }

        public async static Task<Realm> CreateAsync(this Realm realm)
        {
            if (realm.Representation == null)
            {
                throw new KeycloakClientFluentInterfaceException("Realm representation cannot be null when creating a realm.");
            }

            var json = JsonConvert.SerializeObject(realm.Representation);
            var stream = json.ToStream();

            await realm.Client.AdminRealmsPostAsync(stream);
            return realm;
        }

        public async static Task<Realm> UpdateAsync(this Realm realm)
        {
            if (realm.Representation == null)
            {
                throw new KeycloakClientFluentInterfaceException("Realm representation cannot be null when updating a realm.");
            }

            await realm.Client.AdminRealmsPutAsync(realm.Name, realm.Representation);
            return realm;
        }

        public async static Task<Realm> DeleteAsync(this Realm realm)
        {
            if (realm.Representation == null)
            {
                throw new KeycloakClientFluentInterfaceException("Realm representation cannot be null when updating a realm.");
            }

            await realm.Client.AdminRealmsDeleteAsync(realm.Name);
            return realm;
        }
    }
}
