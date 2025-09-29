namespace Keycloak.ApiClient.ConsoleApp;

using Keycloak.ApiClient;
using Keycloak.ApiClient.FluentInterface;

internal class Program
{
    static void Main(string[] args)
    {
        var tmp = new TestKeyCloak();
        tmp.Test().GetAwaiter().GetResult();
    }
}

public class TestKeyCloak()
{
    private string keycloakUrl = "";
    private string username = "";
    private string password = "";

    private string realmName = "test-code";
    private string clientName = "client-code";
    private string clientId = "client-code";


    public async Task Test()
    {
        var kcClient = await KeycloakApiClientFactory.GetFluentKeycloakApiClientAsync(keycloakUrl, username, password);
        var realm = await kcClient.GetRealmAsync(realmName);
        var client = await realm.GetClientAsync(clientId);
        await client.DeleteAsync();
        client = await realm.CreateClientAsync(clientName);
        await client.GetRotatedClientSecret();


        var json = ToJson(client);
        await File.WriteAllTextAsync($"../../../client-{clientName}.json", json);
        Console.WriteLine(json);
    }

    private string ToJson(object obj)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        return json;
    }
}
