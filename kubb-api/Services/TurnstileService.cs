using System.Text.Json;
using System.Text.Json.Serialization;

namespace KubbAdminAPI.Services;

using System.Text;

public class TurnstileService(IConfiguration configuration) {

    private readonly IConfiguration _configuration = configuration;

    /**
     *  Verify Cloudflare Turnstile using httpClient
     */
    public bool VerifyTurnstile(string? token, string ipAddress) {
        if (!_configuration.GetValue<bool>("SystemConfiguration:EnableTurnstile")) return true;
        if (token == null) return false;
        var secret = _configuration.GetValue<string>("Turnstile:Secret")!;
        var task = Task.Run(async() => await VerifyTurnstileAsync(token, ipAddress, secret));
        task.Wait();
        return task.Result;
    }


    private async static Task<bool> VerifyTurnstileAsync(string token, string ipAddress, string cfSecret)
    {

        var httpClient = new HttpClient();

        var request = new TurnstileRequest
        {
            Secret = cfSecret,
            RemoteIp = ipAddress,
            Response = token
        };

        var jsonBody = JsonSerializer.Serialize(request);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");


        var rawResponse = await httpClient.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", content);

        var result = await rawResponse.Content.ReadAsStringAsync();
        
        var turnstileResponse = JsonSerializer.Deserialize<TurnstileResponse>(result);

        return turnstileResponse!.Success;

    }

    private class TurnstileRequest
    {
        [JsonPropertyName("secret")]
        public string Secret { get; set; } = "";

        [JsonPropertyName("response")]
        public string Response { get; set; } = "";

        [JsonPropertyName("remoteip")]
        public string RemoteIp { get; set; } = "";
    }

    private class TurnstileResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}