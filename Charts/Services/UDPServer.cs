using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using Charts.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace Charts.Services
{ 
    public class UDPServer : BackgroundService
    {
        private const int _bytesCount = 1924;
        private const int _bytesACCount = 6804;

        public const int PORT_1_5 = 30100;
        public const int PORT_2_4 = 30101;
        public const int PORT_5_2 = 30102;
        public const int PORT_5_8 = 30103;

        public const int PORT_AC = 30200;

        private readonly IHubContext<UdpHub> _hub;

        private float[] _sendSamples15 = new float[480];
        private float[] _sendSfarSamples15 = new float[480];

        private float[] _sendSamples24 = new float[480];
        private float[] _sendSfarSamples24 = new float[480];

        private float[] _sendSamples52 = new float[480];
        private float[] _sendSfarSamples52 = new float[480];

        private float[] _sendSamples58 = new float[480];
        private float[] _sendSfarSamples58 = new float[480];

        private float[] _sendSamplesAC = new float[850];
        private float[] _sendUPRSamplesAC = new float[850];

        private int _azAnScan;
        private int _azAn;



        private Socket _socket15;
        private EndPoint _eP15;

        private Socket _socket24;
        private EndPoint _eP24;

        private Socket _socket52;
        private EndPoint _eP52;

        private Socket _socket58;
        private EndPoint _eP58;

        private Socket _socketAC;
        private EndPoint _ePAC;

        private byte[] _buffer_recv;
        private byte[] _buffer_recvAC;

        private ArraySegment<byte> _buffer_recv_segment15;
        private ArraySegment<byte> _buffer_recv_segment24;
        private ArraySegment<byte> _buffer_recv_segment52;
        private ArraySegment<byte> _buffer_recv_segment58;
        private ArraySegment<byte> _buffer_recv_segmentAC;


        public UDPServer(IHubContext<UdpHub> hub)
        {
            _hub = hub;

            Initialize();
            StartListeningPortsLoop();
        }

        public void Initialize()
        { 
            _buffer_recv = new byte[_bytesCount];
            _buffer_recvAC = new byte[_bytesACCount];


            _buffer_recv_segment15 = new(_buffer_recv);
            _buffer_recv_segment24 = new(_buffer_recv);
            _buffer_recv_segment52 = new(_buffer_recv);
            _buffer_recv_segment58 = new(_buffer_recv);

            _buffer_recv_segmentAC = new(_buffer_recvAC);


            _eP15 = new IPEndPoint(IPAddress.Any, PORT_1_5);
            _eP24 = new IPEndPoint(IPAddress.Any, PORT_2_4);
            _eP52 = new IPEndPoint(IPAddress.Any, PORT_5_2);
            _eP58 = new IPEndPoint(IPAddress.Any, PORT_5_8);
            _ePAC = new IPEndPoint(IPAddress.Any, PORT_AC);


            _socket15 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket24 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket52 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket58 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socketAC = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


            _socket15.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket24.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket52.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket58.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socketAC.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);


            _socket15.Bind(_eP15);
            _socket24.Bind(_eP24);
            _socket52.Bind(_eP52);
            _socket58.Bind(_eP58);
            _socketAC.Bind(_ePAC);

        }

        public void StartListeningPortsLoop()
        {
            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult resAC;

                while (true)
                {
                    resAC = await _socketAC.ReceiveMessageFromAsync(_buffer_recv_segmentAC, SocketFlags.None, _ePAC);

                    for (int i = 0; i < (_buffer_recv_segmentAC.ToArray().Length - 4) / 2; i += 4)
                    {
                        _sendSamplesAC[i / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC.ToArray(), i);
                    }

                    for (int i = (_buffer_recv_segmentAC.ToArray().Length - 4) / 2; i < (_buffer_recv_segmentAC.ToArray().Length - 4); i += 4)
                    {
                        _sendUPRSamplesAC[i / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC.ToArray(), i);
                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult res15;
               
                while (true)
                {
                    res15 = await _socket15.ReceiveMessageFromAsync(_buffer_recv_segment15, SocketFlags.None, _eP15);

                    for (int i = 0; i < _buffer_recv_segment24.ToArray().Length - 4; i += 4)
                    {
                        _sendSamples15[i / 4] = BitConverter.ToSingle(_buffer_recv_segment15.ToArray(), i);
                    }

                    for (int i = 0; i < _sendSfarSamples24.Length; i++)
                    {
                        _sendSfarSamples15[i] = BitConverter.ToSingle(_buffer_recv_segment15.ToArray(), _buffer_recv_segment15.ToArray().Length - 4);                      
                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {             
                SocketReceiveMessageFromResult res24;
               
                while (true)
                {
                    res24 = await _socket24.ReceiveMessageFromAsync(_buffer_recv_segment24, SocketFlags.None, _eP24);



                    for (int i = 0; i < _buffer_recv_segment24.ToArray().Length - 4; i += 4)
                    {                    
                        _sendSamples24[i / 4] = BitConverter.ToSingle(_buffer_recv_segment24.ToArray(), i);                    
                    }

                    for (int i = 0; i < _sendSfarSamples24.Length; i++)
                    {
                        _sendSfarSamples24[i] = BitConverter.ToSingle(_buffer_recv_segment24.ToArray(), _buffer_recv_segment24.ToArray().Length - 4);
                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult res52;

                while (true)
                {               
                    res52 = await _socket52.ReceiveMessageFromAsync(_buffer_recv_segment52, SocketFlags.None, _eP52);

                    for (int i = 0; i < _buffer_recv_segment24.ToArray().Length - 4; i += 4)
                    {
                        _sendSamples52[i / 4] = BitConverter.ToSingle(_buffer_recv_segment52.ToArray(), i);
                    }

                    for (int i = 0; i < _sendSfarSamples24.Length; i++)
                    {                 
                        _sendSfarSamples52[i] = BitConverter.ToSingle(_buffer_recv_segment52.ToArray(), _buffer_recv_segment52.ToArray().Length - 4);
                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {             
                SocketReceiveMessageFromResult res58;

                while (true)
                {
                  
                    res58 = await _socket58.ReceiveMessageFromAsync(_buffer_recv_segment58, SocketFlags.None, _eP58);


                    for (int i = 0; i < _buffer_recv_segment24.ToArray().Length - 4; i += 4)
                    {                      
                        _sendSamples58[i / 4] = BitConverter.ToSingle(_buffer_recv_segment58.ToArray(), i);
                    }

                    for (int i = 0; i < _sendSfarSamples24.Length; i++)
                    {                       
                        _sendSfarSamples58[i] = BitConverter.ToSingle(_buffer_recv_segment58.ToArray(), _buffer_recv_segment58.ToArray().Length - 4);
                    }
                }
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
             while (!stoppingToken.IsCancellationRequested)      
             {
                _azAnScan+=5;

                if (_azAnScan>360)
                {
                    _azAnScan = 0;
                    _azAn += 10;
                }
                if (_azAn > 360)
                { _azAn = 0;}

                await _hub.Clients.All.SendAsync(
                        "addChartData",
                        new PointRF(_sendSamples15, _sendSfarSamples15, _sendSamples24, _sendSfarSamples24,
                                    _sendSamples52, _sendSfarSamples52, _sendSamples58, _sendSfarSamples58, 
                                    _sendSamplesAC, _sendUPRSamplesAC, _azAnScan, _azAn),
                        cancellationToken: stoppingToken
                    );

                    await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);         
             }
        }
    }
}
