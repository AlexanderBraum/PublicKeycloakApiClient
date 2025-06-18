using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class AdminEvent
    {
        public string Id => Representation?.Id ?? string.Empty;
        public AdminEventRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public AdminEvent(
            Realm realm)
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<AdminEvent>> GetAllAdminEventsAsync(this Realm realm, string authClient = null, string authIpAddress = null, string authRealm = null, string authUser = null, string dateFrom = null, string dateTo = null, string direction = null, int? first = null, int? max = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAdminEventsGetAsync(realm: realm.Name, authClient: authClient, authIpAddress: authIpAddress, authRealm: authRealm, authUser: authUser, dateFrom: dateFrom, dateTo: dateTo, direction: direction, first: first, max: max);
            var result = data.Result.Select(x => realm.GetAdminEventObject(x)).ToList();
            return result;
        }

        public async static Task DeleteAllAdminEventsAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsAdminEventsDeleteAsync(realm.Name);
        }

        private static AdminEvent GetAdminEventObject(this Realm realm, AdminEventRepresentation representation)
        {
            var result = new AdminEvent(realm)
            {
                Representation = representation
            };
            return result;
        }
    }
}