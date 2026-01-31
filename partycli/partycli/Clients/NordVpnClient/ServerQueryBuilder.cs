using partycli.Clients.NordVpnClient.Enums;
using System.Collections.Generic;
using System.Linq;

namespace partycli.Clients.NordVpnClient
{
    internal class ServerQueryBuilder
    {
        private readonly Dictionary<string, string> _filters = new Dictionary<string, string>();

        public ServerQueryBuilder WithCountry(int countryId)
        {
            _filters["filters[country_id]"] = countryId.ToString();
            return this;
        }

        public ServerQueryBuilder WithTechnology(int technologyId)
        {
            _filters["filters[servers_technologies][id]"] = technologyId.ToString();
            return this;
        }

        public ServerQueryBuilder WithProtocol(VpnProtocol protocol)
        {
            return WithTechnology((int)protocol);
        }

        public ServerQueryBuilder WithLimit(int limit)
        {
            _filters["limit"] = limit.ToString();
            return this;
        }

        public string Build()
        {
            if (_filters.Count == 0)
                return string.Empty;

            return "?" + string.Join("&", _filters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }
}
