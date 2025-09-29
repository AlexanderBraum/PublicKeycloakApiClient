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
        public FluentKeycloakApiClient Client { get; }
        public RealmRepresentation Representation { get; set; }

        public Realm(
            FluentKeycloakApiClient client)
        {
            Client = client;
        }
    }

    public static partial class FluentKeycloakApiClientExtensions
    {
        public async static Task<ICollection<Realm>> GetAllRealmsAsync(this FluentKeycloakApiClient client, bool? briefRepresentation = null)
        {
            var data = await client.GeneratedClient.AdminRealmsGetAsync(briefRepresentation);
            var result = data.Result.Select(x => client.GetRealmObject(x)).ToList();
            return result;
        }

        public async static Task<Realm> GetRealmAsync(this FluentKeycloakApiClient client, string realm)
        {
            var data = await client.GeneratedClient.AdminRealmsGetAsync(realm);
            var result = client.GetRealmObject(data.Result);
            return result;
        }

        public async static Task<Realm> CreateRealmAsync(
            this FluentKeycloakApiClient client,
            RealmRepresentation realmRepresentation)
        {
            var realm = client.GetRealmObject(realmRepresentation);
            realm = await realm.CreateAsync();
            return realm;
        }

        public async static Task<Realm> CreateRealmAsync(
            this FluentKeycloakApiClient client,
            string realmName)
        {

            var realm = await client.CreateRealmAsync(GetDefaultRealmRepresentation(realmName));
            return realm;
        }

        private static Realm GetRealmObject(this FluentKeycloakApiClient client, RealmRepresentation realmRepresentation)
        {
            var result = new Realm(client)
            {
                Representation = realmRepresentation
            };
            return result;
        }

        public static RealmRepresentation GetDefaultRealmRepresentation(string realmName)
        {
            return new RealmRepresentation
            {
                Id = realmName,
                Realm = realmName,
                DisplayName = realmName,
                DisplayNameHtml = null,
                DefaultSignatureAlgorithm = "RS256",
                Enabled = true,
                SslRequired = "external",
                AccessTokenLifespan = 300,
                AccessTokenLifespanForImplicitFlow = 900,
                SsoSessionIdleTimeout = 1800,
                SsoSessionMaxLifespan = 36000,
                SsoSessionIdleTimeoutRememberMe = 0,
                SsoSessionMaxLifespanRememberMe = 0,
                OfflineSessionIdleTimeout = 2592000,
                OfflineSessionMaxLifespanEnabled = false,
                OfflineSessionMaxLifespan = 5184000,
                ClientSessionIdleTimeout = 0,
                ClientSessionMaxLifespan = 0,
                ClientOfflineSessionIdleTimeout = 0,
                ClientOfflineSessionMaxLifespan = 0,
                AccessCodeLifespan = 60,
                AccessCodeLifespanUserAction = 300,
                AccessCodeLifespanLogin = 1800,
                ActionTokenGeneratedByAdminLifespan = 43200,
                ActionTokenGeneratedByUserLifespan = 300,
                Oauth2DevicePollingInterval = 5,
                RegistrationAllowed = false,
                RegistrationEmailAsUsername = false,
                RememberMe = false,
                VerifyEmail = false,
                LoginWithEmailAllowed = true,
                DuplicateEmailsAllowed = false,
                ResetPasswordAllowed = false,
                EditUsernameAllowed = false,
                BruteForceProtected = false,
                PermanentLockout = false,
                MaxTemporaryLockouts = 0,
                BruteForceStrategy = BruteForceStrategy.MULTIPLE,
                MaxFailureWaitSeconds = 900,
                MinimumQuickLoginWaitSeconds = 60,
                WaitIncrementSeconds = 60,
                QuickLoginCheckMilliSeconds = 1000,
                MaxDeltaTimeSeconds = 43200,
                FailureFactor = 30,
                DefaultRole = new RoleRepresentation
                {
                    Name = $"default-roles-{realmName}",
                    Description = $"default-roles-{realmName}",
                    Composite = true,
                    ClientRole = false,
                },
                OtpPolicyType = "totp",
                OtpPolicyAlgorithm = "HmacSHA1",
                OtpPolicyInitialCounter = 0,
                OtpPolicyDigits = 6,
                OtpPolicyLookAheadWindow = 1,
                OtpPolicyPeriod = 30,
                OtpPolicyCodeReusable = false,
                OtpSupportedApplications = new List<string>
                {
                    "totpAppFreeOTPName",
                    "totpAppGoogleName",
                    "totpAppMicrosoftAuthenticatorName"
                },
                WebAuthnPolicyRpEntityName = "keycloak",
                WebAuthnPolicySignatureAlgorithms = new List<string> { "ES256", "RS256" },
                WebAuthnPolicyRpId = "",
                WebAuthnPolicyAttestationConveyancePreference = "not specified",
                WebAuthnPolicyAuthenticatorAttachment = "not specified",
                WebAuthnPolicyRequireResidentKey = "not specified",
                WebAuthnPolicyUserVerificationRequirement = "not specified",
                WebAuthnPolicyCreateTimeout = 0,
                WebAuthnPolicyAvoidSameAuthenticatorRegister = false,
                WebAuthnPolicyAcceptableAaguids = new List<string>(),
                WebAuthnPolicyExtraOrigins = new List<string>(),
                WebAuthnPolicyPasswordlessRpEntityName = "keycloak",
                WebAuthnPolicyPasswordlessSignatureAlgorithms = new List<string> { "ES256", "RS256" },
                WebAuthnPolicyPasswordlessRpId = "",
                WebAuthnPolicyPasswordlessAttestationConveyancePreference = "not specified",
                WebAuthnPolicyPasswordlessAuthenticatorAttachment = "not specified",
                WebAuthnPolicyPasswordlessRequireResidentKey = "not specified",
                WebAuthnPolicyPasswordlessUserVerificationRequirement = "not specified",
                WebAuthnPolicyPasswordlessCreateTimeout = 0,
                WebAuthnPolicyPasswordlessAvoidSameAuthenticatorRegister = false,
                WebAuthnPolicyPasswordlessAcceptableAaguids = new List<string>(),
                WebAuthnPolicyPasswordlessExtraOrigins = new List<string>(),
                ClientProfiles = new ClientProfilesRepresentation
                {
                    Profiles = new List<ClientProfileRepresentation>(),
                    AdditionalProperties = new Dictionary<string, object>()
                },
                BrowserSecurityHeaders = new Dictionary<string, string>
                {
                    {  "contentSecurityPolicyReportOnly", "" },
                    {  "xContentTypeOptions", "nosniff" },
                    {  "referrerPolicy", "no-referrer" },
                    {  "xRobotsTag", "none" },
                    {  "xFrameOptions", "SAMEORIGIN" },
                    {  "contentSecurityPolicy", "frame-src \u0027self\u0027; frame-ancestors \u0027self\u0027; object-src \u0027none\u0027;" },
                    {  "strictTransportSecurity", "max-age=31536000; includeSubDomains" },
                },
                BrowserFlow = "browser",
                RegistrationFlow = "registration",
                DirectGrantFlow = "direct grant",
                ResetCredentialsFlow = "reset credentials",
                ClientAuthenticationFlow = "clients",
                DockerAuthenticationFlow = "docker auth",
                FirstBrokerLoginFlow = "first broker login",
                Attributes = new Dictionary<string, string>
                {
                    { "cibaBackchannelTokenDeliveryMode", "poll" },
                    { "cibaExpiresIn", "120" },
                    { "cibaAuthRequestedUserHint", "login_hint" },
                    { "oauth2DeviceCodeLifespan", "600" },
                    { "oauth2DevicePollingInterval", "5" },
                    { "parRequestUriLifespan", "60" },
                    { "cibaInterval", "5" },
                    { "realmReusableOtpCode", "false" }
                },
                EventsEnabled = false,
                EventsExpiration = 0,
                EventsListeners = new List<string> { "jboss-logging" },
                EnabledEventTypes = new List<string>(),
                AdminEventsEnabled = false,
                AdminEventsDetailsEnabled = false,
                InternationalizationEnabled = false,
                UserManagedAccessAllowed = false,
                OrganizationsEnabled = false,
                VerifiableCredentialsEnabled = false,
                AdminPermissionsEnabled = false,
                AdditionalProperties = new Dictionary<string, object>
                {
                    { "oauth2DeviceCodeLifespan",600}
                }
            };
        }
    }

    public static partial class RealmExtensions
    {
        public async static Task<Realm> GetAsync(this Realm realm)
        {
            var result = await realm.Client.GeneratedClient.AdminRealmsGetAsync(realm.Name);
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

            await realm.Client.GeneratedClient.AdminRealmsPostAsync(stream);
            return realm;
        }

        public async static Task<Realm> UpdateAsync(this Realm realm)
        {
            if (realm.Representation == null)
            {
                throw new KeycloakClientFluentInterfaceException("Realm representation cannot be null when updating a realm.");
            }

            await realm.Client.GeneratedClient.AdminRealmsPutAsync(realm.Name, realm.Representation);
            return realm;
        }

        public async static Task<Realm> DeleteAsync(this Realm realm)
        {
            if (realm.Representation == null)
            {
                throw new KeycloakClientFluentInterfaceException("Realm representation cannot be null when updating a realm.");
            }

            await realm.Client.GeneratedClient.AdminRealmsDeleteAsync(realm.Name);
            return realm;
        }
    }
}
