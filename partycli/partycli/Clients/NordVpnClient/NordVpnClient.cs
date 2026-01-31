using Newtonsoft.Json;
using partycli.Clients.NordVpnClient.Enums;
using partycli.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace partycli.Clients.NordVpnClient
{
    internal class NordVpnClient
    {
        private readonly HttpClient _client;
        private readonly NordVpnConfig _config;

        public NordVpnClient(HttpClient client = null, NordVpnConfig config = null)
        {
            _client = client ?? new HttpClient();
            _config = config ?? new NordVpnConfig();
            _client.Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds);
        }

        public async Task<IEnumerable<ServerDto>> GetServersAsync(ServerQueryBuilder queryBuilder = null)
        {
            string query = queryBuilder?.Build() ?? string.Empty;
            string endpoint = $"{_config.BaseUrl}/servers{query}";

            return await ExecuteGetAsync<ServerDto>(endpoint);
        }

        public Task<IEnumerable<ServerDto>> GetAllServersAsync()
        {
            return GetServersAsync();
        }

        public Task<IEnumerable<ServerDto>> GetServersByCountryAsync(int countryId)
        {
            ServerQueryBuilder query 
                = new ServerQueryBuilder()
                    .WithCountry(countryId)
                    .WithTechnology(35);

            return GetServersAsync(query);
        }

        public Task<IEnumerable<ServerDto>> GetServersByProtocolAsync(VpnProtocol protocol)
        {
            ServerQueryBuilder query 
                = new ServerQueryBuilder().WithProtocol(protocol);

            return GetServersAsync(query);
        }

        private async Task<IEnumerable<TResponse>> ExecuteGetAsync<TResponse>(
            string endpoint, 
            int retryCount = 3)
        {
            Exception lastException = null;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                    HttpResponseMessage response = await _client.SendAsync(request);

                    response.EnsureSuccessStatusCode();

                    string reponse = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<IEnumerable<TResponse>>(reponse);
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;

                    if (i < retryCount - 1)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i)));
                    }
                }
            }

            throw new Exception($"Failed to fetch from {endpoint} after {retryCount} attempts", lastException);
        }
    }
}