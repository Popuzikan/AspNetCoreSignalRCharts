using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using Charts.Interfaces;
using Charts.ServicesAbstract;
using Charts.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace Charts.Services
{ 
    public class UDPServer : BackgroundService, ITransmiter
    {
        private const int _bytesCount = 1924;
        public const int PORT_2_4 = 30101;

        private readonly IHubContext<ChartHub> _hub;


        private float[] _sendSamples = new float[480];
        private float[] _sendSfarSamples = new float[480];


        private Socket _socket;
        private EndPoint _eP;

        private byte[] _buffer_recv;
        private ArraySegment<byte> _buffer_recv_segment;

        public event EventHandler<TransmiteDateArgs> SendingDate;


        public UDPServer(IHubContext<ChartHub> hub)
        {
            _hub = hub;

            Initialize();
            StartListeningPortsLoop();
        }

        public void Initialize()
        { 
            _buffer_recv = new byte[_bytesCount];
            _buffer_recv_segment = new(_buffer_recv);

            _eP = new IPEndPoint(IPAddress.Any, PORT_2_4);

            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram,
                          ProtocolType.Udp);

            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);

            _socket.Bind(_eP);
        }

        public void StartListeningPortsLoop()
        {

            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult res;

                while (true)
                {
                    res = await _socket.ReceiveMessageFromAsync(
                        _buffer_recv_segment, SocketFlags.None, _eP);

                    for (int i = 0; i < _buffer_recv_segment.ToArray().Length - 4; i+=4)
                          _sendSamples[i/4] = BitConverter.ToSingle(_buffer_recv_segment.ToArray(), i);

                    for (int i = 0; i < _sendSfarSamples.Length; i++)
                    {
                        _sendSfarSamples[i] = BitConverter.ToSingle(_buffer_recv_segment.ToArray(), _buffer_recv_segment.ToArray().Length - 4);
                    }
                    
                }
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
             while (!stoppingToken.IsCancellationRequested)      
             {              
                await _hub.Clients.All.SendAsync(
                        "addChartData",
                        new PointRF(_sendSamples, _sendSfarSamples),
                        cancellationToken: stoppingToken
                    );

                    await Task.Delay(TimeSpan.FromSeconds(0.05), stoppingToken);         
             }
        }
    }
}
