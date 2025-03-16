namespace KubbAdminAPI.Services;

using System.Text;
using Newtonsoft.Json;

public class TurnstileService(IConfiguration configuration) {

    private readonly IConfiguration _configuration = configuration;

    /**
     *  Verify Cloudflare Turnstile using httpClient
     */
    public bool VerifyTurnstile(string token, string ipAddress) {
        if (!_configuration.GetValue<bool>("SystemConfiguration:EnableTurnstile")) return true;
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

        var jsonBody = JsonConvert.SerializeObject(request);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");


        var rawResponse = await httpClient.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", content);

        var result = await rawResponse.Content.ReadAsStringAsync();
        
        var turnstileResponse = JsonConvert.DeserializeObject<TurnstileResponse>(result);

        Console.WriteLine(result);
        Console.WriteLine(jsonBody);

        return turnstileResponse!.Success;

    }

    private class TurnstileRequest
    {
        [JsonProperty(PropertyName = "secret")]
        public string Secret { get; set; } = "";

        [JsonProperty(PropertyName = "response")]
        public string Response { get; set; } = "";

        [JsonProperty(PropertyName = "remoteip")]
        public string RemoteIp { get; set; } = "";
    }

    private class TurnstileResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }
}