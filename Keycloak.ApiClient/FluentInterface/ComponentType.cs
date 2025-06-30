
using keycloak;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class ComponentType
    {
        public string Id => Representation?.Id ?? string.Empty;
        public ComponentTypeRepresentation Representation { get; set; }
        public Realm Realm { get; set; }

        public ComponentType(
            Realm realm
            )
        {
            Realm = realm;
        }
    }

    public class SubComponentType
    {
        public string Id => Representation?.Id ?? string.Empty;
        public ComponentTypeRepresentation Representation { get; set; }
        public Component Component { get; set; }

        public SubComponentType(
            Component component
            )
        {
            Component = component;
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<ICollection<ComponentType>> GetAllClientRegistrationPolicyProvidersAsync(this Realm realm)
        {
            var data = await realm.Client.GeneratedClient.AdminRealmsClientRegistrationPolicyProvidersAsync(realm.Name);
            var result = data.Result.Select(x => realm.GetComponentTypeObject(x)).ToList();
            return result;
        }

        private static ComponentType GetComponentTypeObject(this Realm realm, ComponentTypeRepresentation representation)
        {
            var result = new ComponentType(realm)
            {
                Representation = representation
            };
            return result;
        }
    }

    public static partial class ComponentExtensions
    {
        public async static Task<ICollection<SubComponentType>> GetAllComponentsSubComponentTypesAsync(
            this Component component,
            string type = null)
        {
            var data = await component.Realm.Client.GeneratedClient.AdminRealmsComponentsSubComponentTypesAsync(id: component.Id, realm: component.Realm.Name, type: type);
            var result = data.Result.Select(x => component.GetSubComponentTypeObject(x)).ToList();
            return result;
        }

        private static SubComponentType GetSubComponentTypeObject(this Component component, ComponentTypeRepresentation representation)
        {
            var result = new SubComponentType(component)
            {
                Representation = representation
            };
            return result;
        }
    }
}