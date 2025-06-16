using keycloak;
using Keycloak.ApiClient.FluentInterface;
using Keycloak.ApiClient.FluentInterface.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Keycloak.ApiClient.Digid
{
    public static class KeycloakApiClientAddDigidExtensions
    {
        public async static Task AddIdentityProviderDigidAsync(
            this KeycloakApiClient keycloakApiClient,
            string realm,
            string clientName,
            string keycloakUrl,
            string[] redirectUrls,
            string[] webOrigins,
            string[] postLogoutRedirectUrls,
            (string Name, string Value)[] clientOidcHardcodedClaimMappers,
            string metadataUrl,
            X509Certificate2 cert
            )
        {
            if (keycloakApiClient == null)
            {
                throw new ArgumentNullException(nameof(keycloakApiClient), "KeycloakApiClient cannot be null.");
            }

            var realmObj = await keycloakApiClient.GetRealmAsync(realm);
            var clientRep = GetClientRepresentation(clientName, keycloakUrl, redirectUrls, webOrigins, postLogoutRedirectUrls);
            var clientObj = await realmObj.CreateClientAsync(clientRep);

            await realmObj.EnsureCertIsUsedForSigningAsync2(clientName, cert);

            foreach (var item in clientOidcHardcodedClaimMappers)
            {
                var proMaRep = GetProtocolMapperRepresentation(item.Name, item.Value);
                await clientObj.CreateProtocolMapperAsync(proMaRep);
            }

            var metadata = await Saml2MetadataExtracter.ExtractAsync(metadataUrl);

            var idpRep = GetIdentityProviderRepresentation(
                "digid",
                metadata.SingleLogoutServiceUrl,
                metadata.IdpEntityId,
                metadata.SingleSignOnServiceUrl,
                metadata.ArtifactResolutionServiceUrl,
                "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified",
                $"{keycloakUrl}/realms/{realm}",
                metadataUrl
            );

            await realmObj.CreateIdentityProviderAsync(idpRep);
        }

        public static ClientRepresentation GetClientRepresentation(
            string clientName,
            string rootUrl,
            string[] redirectUrls,
            string[] webOrigins,
            string[] postLogoutRedirectUrls
            )
        {
            var dto = new ClientRepresentation
            {
                ClientId = clientName,
                Name = clientName,
                RootUrl = rootUrl,
                AdminUrl = rootUrl,
                BaseUrl = rootUrl,
                RedirectUris = redirectUrls,
                WebOrigins = webOrigins,
                Enabled = true,
                ClientAuthenticatorType = "client-secret",
                ImplicitFlowEnabled = true,
                DirectAccessGrantsEnabled = true,
                PublicClient = true,
                FrontchannelLogout = true,
                Attributes = new Dictionary<string, string>
                    {
                        { "post.logout.redirect.uris", string.Join("##", postLogoutRedirectUrls) }
                    }
            };
            return dto;
        }

        public async static Task EnsureCertIsUsedForSigningAsync2(
            this Realm realm,
            string name,
            X509Certificate2 cert
            )
        {
            var componants = await realm.GetAllComponentsAsync();
            await componants.SingleOrDefault(x => x.Name == name)
                .DeleteAsync();

            var componantRep = GetComponentRepresentationForRsaCert(cert);
            await realm.CreateComponentAsync(componantRep);
        }

        public static ComponentRepresentation GetComponentRepresentationForRsaCert(X509Certificate2 cert)
        {
            var (Certificate, PrivateKey) = ReadPfxFile(cert);
            var dto = new ComponentRepresentation
            {
                Name = "rsa",
                ProviderId = "rsa",
                ProviderType = "org.keycloak.keys.KeyProvider",
                Config = new MultivaluedHashMapStringString()
                    {
                        { "priority", new Collection<string>{ "110" } },
                        { "enabled", new Collection<string>{ "true" } },
                        { "active", new Collection<string>{ "true" } },
                        { "privateKey", new Collection<string>{ PrivateKey } },
                        { "certificate", new Collection<string>{ Certificate } },
                        { "algorithm", new Collection<string>{ "RS256" } }
                    }
            };
            return dto;
        }

        public static (string Certificate, string PrivateKey) ReadPfxFile(X509Certificate2 cert)
        {
            var certString = Convert.ToBase64String(cert.Export(X509ContentType.Cert));
            var privateKey = cert.GetRSAPrivateKey();
            var privateKeyString = Convert.ToBase64String(privateKey.ExportRSAPrivateKey());
            return (certString, privateKeyString);
        }

        public static ProtocolMapperRepresentation GetProtocolMapperRepresentation(string name, string claimValue)
        {
            var dto = new ProtocolMapperRepresentation
            {
                Name = name,
                Protocol = "openid-connect",
                ProtocolMapper = "oidc-hardcoded-claim-mapper",
                Config = new Dictionary<string, string>
                    {
                        { "claim.value", claimValue },
                    }
            };
            return dto;
        }

        public static IdentityProviderRepresentation GetIdentityProviderRepresentation(
            string alias,
            string singleLogoutServiceUrl,
            string idpEntityId,
            string singleSignOnServiceUrl,
            string artifactResolutionServiceUrl,
            string nameIDPolicyFormat,
            string entityId,
            string metadataDescriptorUrl)
        {
            var dto = new IdentityProviderRepresentation
            {
                Alias = alias,
                DisplayName = alias,
                ProviderId = "saml",
                AddReadTokenRoleOnCreate = true,
                Config = new Dictionary<string, string>
                    {
                        { "postBindingLogout", "false" },
                        { "postBindingResponse", "true" },
                        { "singleLogoutServiceUrl", singleLogoutServiceUrl },
                        { "backchannelSupported", "true" },
                        { "caseSensitiveOriginalUsername", "false" },
                        { "xmlSigKeyInfoKeyNameTransformer", "KEY_ID" },
                        { "idpEntityId", idpEntityId },
                        { "loginHint", "true" },
                        { "allowCreate", "true" },
                        { "enabledFromMetadata", "true" },
                        { "authnContextComparisonType", "exact" },
                        { "syncMode", "LEGACY" },
                        { "singleSignOnServiceUrl", singleSignOnServiceUrl },
                        { "wantAuthnRequestsSigned", "true" },
                        { "allowedClockSkew", "0" },
                        { "artifactResolutionServiceUrl", artifactResolutionServiceUrl },
                        { "artifactBindingResponse", "true" },
                        { "validateSignature", "false" },
                        { "nameIDPolicyFormat", nameIDPolicyFormat },
                        { "entityId", entityId },
                        { "signSpMetadata", "true" },
                        { "signatureAlgorithm", "RSA_SHA256" },
                        { "wantAssertionsEncrypted", "false" },
                        { "sendClientIdOnLogout", "false" },
                        { "metadataDescriptorUrl", metadataDescriptorUrl },
                        { "wantAssertionsSigned", "true" },
                        { "sendIdTokenOnLogout", "true" },
                        { "postBindingAuthnRequest", "true" },
                        { "forceAuthn", "false" },
                        { "attributeConsumingServiceIndex", "0" },
                        { "addExtensionsElementWithKeyInfo", "false" },
                        { "principalType", "SUBJECT" }
                    }
            };
            return dto;
        }
    }
}
