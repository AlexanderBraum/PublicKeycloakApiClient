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
        /// <summary>
        /// Adds a DigiD Identity Provider to the specified Keycloak realm. The name of the IDP will be "digid".
        /// </summary>
        /// <param name="realm">The realm where to add the DigiD IDP.</param>
        /// <param name="client">The client used for brokering to the Logius (DigiD) IDP.</param>
        /// <param name="keycloakUrl">The URL of your Keycloak instance.</param>
        /// <param name="metadataUrl">The Logius Metadata (SAML2) URL.</param>
        /// <param name="cert">Used for signing the metadata in the SAML2 request.</param>
        /// <returns></returns>
        public async static Task AddIdentityProviderDigidAsync(
            this Realm realm,
            Client client,
            string keycloakUrl,
            string metadataUrl,
            X509Certificate2 cert
            )
        {
            await realm.AddIdentityProviderDigidAsync(
                client,
                keycloakUrl,
                null,
                metadataUrl,
                cert
            );
        }

        /// <summary>
        /// Adds a DigiD Identity Provider to the specified Keycloak realm. The name of the IDP will be "digid".
        /// </summary>
        /// <param name="realm">The realm where to add the DigiD IDP.</param>
        /// <param name="client">The client used for brokering to the Logius (DigiD) IDP.</param>
        /// <param name="keycloakUrl">The URL of your Keycloak instance.</param>
        /// <param name="clientOidcHardcodedClaimMappers">Extra claims to add to a JWT, e.g., with realm the JWT comes from, for a multi-tenant setup.</param>
        /// <param name="metadataUrl">The Logius Metadata (SAML2) URL.</param>
        /// <param name="cert">Used for signing the metadata in the SAML2 request.</param>
        /// <returns></returns>
        public async static Task AddIdentityProviderDigidAsync(
            this Realm realm,
            Client client,
            string keycloakUrl,
            (string Name, string Value)[] clientOidcHardcodedClaimMappers,
            string metadataUrl,
            X509Certificate2 cert
            )
        {
            EnsureInputParams(realm, client, keycloakUrl, metadataUrl, cert);

            await realm.EnsureCertIsUsedForSigningAsync2(cert);

            if (clientOidcHardcodedClaimMappers != null)
            {
                foreach (var item in clientOidcHardcodedClaimMappers)
                {
                    var proMaRep = GetProtocolMapperRepresentation(item.Name, item.Value);
                    await client.CreateProtocolMapperAsync(proMaRep);
                }
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

            await realm.CreateIdentityProviderAsync(idpRep);
        }

        private static void EnsureInputParams(
            Realm realm,
            Client client,
            string keycloakUrl,
            string metadataUrl,
            X509Certificate2 cert)
        {
            if (realm == null)
            {
                throw new ArgumentNullException(nameof(realm), "Realm cannot be null.");
            }
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "client cannot be null.");
            }
            if (keycloakUrl == null)
            {
                throw new ArgumentNullException(nameof(keycloakUrl), "keycloakUrl cannot be null.");
            }
            if (metadataUrl == null)
            {
                throw new ArgumentNullException(nameof(metadataUrl), "keycloakUrl cannot be null.");
            }
            if (cert == null)
            {
                throw new ArgumentNullException(nameof(cert), "cert cannot be null.");
            }
        }

        private async static Task EnsureCertIsUsedForSigningAsync2(
            this Realm realm,
            X509Certificate2 cert
            )
        {
            var components = await realm.GetAllComponentsAsync();
            var component = components.SingleOrDefault(x => x.Name == "rsa-generated");
            if(component != null)
            {
                await component.DeleteAsync();
            }

            var componantRep = GetComponentRepresentationForRsaCert(cert);
            await realm.CreateComponentAsync(componantRep);
        }

        private static ComponentRepresentation GetComponentRepresentationForRsaCert(X509Certificate2 cert)
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

        private static (string Certificate, string PrivateKey) ReadPfxFile(X509Certificate2 cert)
        {
            var certString = Convert.ToBase64String(cert.Export(X509ContentType.Cert));
            var privateKey = cert.GetRSAPrivateKey();
            var privateKeyString = Convert.ToBase64String(privateKey.ExportRSAPrivateKey());
            return (certString, privateKeyString);
        }

        private static ProtocolMapperRepresentation GetProtocolMapperRepresentation(string name, string claimValue)
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

        private static IdentityProviderRepresentation GetIdentityProviderRepresentation(
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
