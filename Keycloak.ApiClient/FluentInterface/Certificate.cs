
using keycloak;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.FluentInterface
{
    public class Certificate
    {
        public string Kid => Representation?.Kid ?? string.Empty;
        public CertificateRepresentation Representation { get; set; }
        public Client Client { get; set; }

        public Certificate(
            Client client
            )
        {
            Client = client;
        }
    }

    public static partial class ClientExtensions
    {
        public async static Task<Certificate> GetCertificateAsync(this Client client, string attr)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsCertificatesAsync(realm: client.Realm.Name, client_uuid: client.Id, attr);
            //var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsCertificatesUploadCertificateAsync
            var result = client.GetCertificateObject(data.Result);
            return result;
        }

        public async static Task<FileResponse> DownloadCertificateAsync(this Client client, string attr, KeyStoreConfig body = null)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsCertificatesDownloadAsync(realm: client.Realm.Name, client_uuid: client.Id, attr: attr, body: body);
            return data;
        }

        public async static Task<FileResponse> GenerateAndDownloadCertificateAsync(this Client client, string attr, KeyStoreConfig body)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsCertificatesGenerateAndDownloadAsync(realm: client.Realm.Name, client_uuid: client.Id, attr: attr, body: body);
            return data;
        }

        public async static Task<Certificate> UploadCertificateAsync(this Client client, string attr)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsCertificatesUploadAsync(realm: client.Realm.Name, client_uuid: client.Id, attr: attr);
            var result = client.GetCertificateObject(data.Result);
            return result;
        }

        public async static Task<Certificate> UploadCertificateNoPrivateKeyAsync(this Client client, string attr)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsCertificatesUploadCertificateAsync(realm: client.Realm.Name, client_uuid: client.Id, attr: attr);
            var result = client.GetCertificateObject(data.Result);
            return result;
        }

        public async static Task<Certificate> GenerateCertificateAsync(this Client client, string attr)
        {
            var data = await client.Realm.Client.GeneratedClient.AdminRealmsClientsCertificatesGenerateAsync(realm: client.Realm.Name, client_uuid: client.Id, attr);
            var result = client.GetCertificateObject(data.Result);
            return result;
        }

        private static Certificate GetCertificateObject(this Client client, CertificateRepresentation representation)
        {
            var result = new Certificate(client)
            {
                Representation = representation
            };
            return result;
        }
    }
}
