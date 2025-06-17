using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class Component
    {
        public string Name => Representation?.Name ?? string.Empty;
        public string Id => Representation?.Id ?? string.Empty;
        public ComponentRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public Component(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<Component>> GetAllComponentsAsync(this Realm realm, string name = null, string parent = null, string type = null)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsComponentsGetAsync(realm.Name, name, parent, type);
            var result = data.Result.Select(x => realm.GetComponentObject(x)).ToList();
            return result;
        }

        public async static Task<Component> GetComponentAsync(this Realm realm, string id)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsComponentsGetAsync(id: id, realm: realm.Name);
            var result = realm.GetComponentObject(data.Result);
            return result;
        }

        public async static Task<Component> CreateComponentAsync(this Realm realm, ComponentRepresentation representation)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsComponentsPostAsync(realm.Name, representation);
            var result = realm.GetComponentObject(representation);
            return result;
        }

        private static Component GetComponentObject(this Realm realm, ComponentRepresentation representation)
        {
            var result = new Component(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ComponentExtensions
    {
        public async static Task<Component> UpdateAsync(this Component obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsComponentsPutAsync(id: obj.Id, realm: obj.Realm.Name, obj.Representation);
            return obj;
        }

        public async static Task<Component> DeleteAsync(this Component obj)
        {
            await obj.Realm.Client.GeneratedClient.AdminRealmsComponentsDeleteAsync(id: obj.Id, realm: obj.Realm.Name);
            return obj;
        }
    }
}
