using Microsoft.AspNetCore.SignalR;

namespace Charts.Hubs
{
    public class UdpHub : Hub
    {
        public const string Url = "/udp";
    }
}
