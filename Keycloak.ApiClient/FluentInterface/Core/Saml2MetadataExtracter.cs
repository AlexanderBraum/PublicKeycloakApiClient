using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Keycloak.ApiClient.FluentInterface.Core
{
    public static class Saml2MetadataExtracter
    {
        public static async Task<SamlUrls> ExtractAsync(string metadataUrl)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            var httpClient = new HttpClient(handler);

            var response = await httpClient.GetAsync(metadataUrl);
            var metadata = await response.Content.ReadAsStringAsync();


            var doc = XDocument.Parse(metadata);

            var singleLogoutServiceUrl = doc.Descendants()
                .Where(x => x.Name.LocalName == "SingleLogoutService")
                .Where(x => x.Attribute("Binding")?.Value == "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect")
                .SingleOrDefault()?
                .Attribute("Location")?.Value ?? "";

            var idpEntityId = doc.Descendants()
                .FirstOrDefault(x => x.Name.LocalName == "EntityDescriptor")?
                .Attribute("entityID")?.Value ?? "";

            var singleSignOnServiceUrl = doc.Descendants()
                .Where(x => x.Name.LocalName == "SingleSignOnService")
                .Where(x => x.Attribute("Binding")?.Value == "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST")
                .SingleOrDefault()?
                .Attribute("Location")?.Value ?? "";

            var artifactResolutionServiceUrl = doc.Descendants()
                .Where(x => x.Name.LocalName == "ArtifactResolutionService"
                        && x.Attribute("Binding")?.Value == "urn:oasis:names:tc:SAML:2.0:bindings:SOAP"
                        && x.Attribute("index")?.Value == "0")
                .SingleOrDefault()?
                .Attribute("Location")?.Value ?? "";

            var result = new SamlUrls
            {
                SingleLogoutServiceUrl = singleLogoutServiceUrl,
                IdpEntityId = idpEntityId,
                SingleSignOnServiceUrl = singleSignOnServiceUrl,
                ArtifactResolutionServiceUrl = artifactResolutionServiceUrl,
            };

            return result;
        }
    }

    public class SamlUrls
    {
        public string SingleLogoutServiceUrl { get; set; }
        public string IdpEntityId { get; set; }
        public string SingleSignOnServiceUrl { get; set; }
        public string ArtifactResolutionServiceUrl { get; set; }
    }
}
