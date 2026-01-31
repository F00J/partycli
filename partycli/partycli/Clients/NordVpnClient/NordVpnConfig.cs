namespace partycli.Clients.NordVpnClient
{
    internal class NordVpnConfig
    {
        public string BaseUrl { get; set; } = "https://api.nordvpn.com/v1";
        public int TimeoutSeconds { get; set; } = 30;
    }
}