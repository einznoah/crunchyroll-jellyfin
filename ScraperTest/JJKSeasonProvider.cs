using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class JJKSeasonProvider
{
    public static async Task Main()
    {
        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://crunchyroll.com/content/v2/cms/series/GRDV0019R/seasons?locale=pt-BR");
        // Needs proper auth or FlareSolverr.
        // Actually, maybe I can just grep the log for "seasons/GR" or check FlareSolverr responses in the log.
    }
}
