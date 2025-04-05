using System.Text;
using System.Text.Json;
using modMnemosyneJSONModels;

namespace modMnemosyneHTTPClient;

public class MnemosyneHTTPClient : IMnemosyneHTTPClient
{
    string serviceUri = "http://localhost:6729";

    public async Task PostMessage(string sender, string receiver, MessageItem message)
    {
        HttpClient httpClient = new HttpClient();
        
        httpClient.DefaultRequestHeaders.Add("Sender", $"{sender}");
        httpClient.DefaultRequestHeaders.Add("Receiver", $"{receiver}");

        string jsonString = JsonSerializer.Serialize(message);
        
        HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage result = await httpClient.PostAsync(serviceUri + "/api/Message", httpContent);
    }

    public async Task<string?> GetTest()
    {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage result = await httpClient.GetAsync(serviceUri + "/api/New");

        try
        {
            if (result.IsSuccessStatusCode)
            {
                var stringData = await result.Content.ReadAsStringAsync();

                return stringData;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("GetTest ERROR: ", ex.ToString());
        }

        return null;
    }
}
