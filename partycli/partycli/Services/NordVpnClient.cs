using System.Net.Http;

namespace partycli.Services
{
    internal static class NordVpnClient
    {
        private const string ServerEndpoint = "https://api.nordvpn.com/v1/servers";

        private static readonly HttpClient client = new HttpClient();

        internal static string GetAllServersListAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ServerEndpoint);
            HttpResponseMessage response = client.SendAsync(request).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        internal static string GetAllServerByCountryListAsync(int countryId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{ServerEndpoint}?filters[servers_technologies][id]=35&filters[country_id]={countryId}");
            HttpResponseMessage response = client.SendAsync(request).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        internal static string GetAllServerByProtocolListAsync(int vpnProtocol)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{ServerEndpoint}?filters[servers_technologies][id]={vpnProtocol}");
            HttpResponseMessage response = client.SendAsync(request).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }
    }
}
