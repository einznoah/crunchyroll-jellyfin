using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

public class JJKEpisodeTest
{
    private const string BaseUrl = "https://www.crunchyroll.com";
    private const string BasicAuthToken = "bmR0aTZicXlqcm9wNXZnZjF0dnU6elpIcS00SEJJVDlDb2FMcnBPREJjRVRCTUNHai1QNlg=";

    public static async Task RunTest()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Crunchyroll/3.50.2");

        var tokenReq = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/auth/v1/token");
        tokenReq.Headers.Authorization = new AuthenticationHeaderValue("Basic", BasicAuthToken);
        tokenReq.Content = new FormUrlEncodedContent(new[]
        {
            new System.Collections.Generic.KeyValuePair<string, string>("grant_type", "client_id")
        });

        var tokenRes = await httpClient.SendAsync(tokenReq);
        var tokenJson = await tokenRes.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(tokenJson);
        var accessToken = doc.RootElement.GetProperty("access_token").GetString();
        
        var apiReq = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/content/v2/cms/seasons/GS00365546JAJP/episodes?locale=pt-BR");
        apiReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var apiRes = await httpClient.SendAsync(apiReq);
        var apiJson = await apiRes.Content.ReadAsStringAsync();
        
        using var apiDoc = JsonDocument.Parse(apiJson);
        foreach (var item in apiDoc.RootElement.GetProperty("data").EnumerateArray())
        {
            var title = item.GetProperty("title").GetString();
            var en = item.GetProperty("episode_number").GetInt32();
            var sen = item.TryGetProperty("sequence_number", out var seqNum) ? seqNum.GetDouble() : 0;
            var sn = item.GetProperty("season_number").GetInt32();
            var ssn = item.HasProperty("season_sequence_number") ? item.GetProperty("season_sequence_number").GetInt32() : 0;
            Console.WriteLine($"E{en} | Seq: {sen} | SN: {sn} | Title: {title}");
        }
    }
}

public static class JsonElementExt
{
    public static bool HasProperty(this JsonElement element, string name) => element.TryGetProperty(name, out _);
}
