using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using Charts.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;
using Charts.StructTypes;

#nullable disable
namespace Charts.Services
{ 
    public class UDPServer : BackgroundService
    {

        public static bool isAutoMode = false;
        private bool isTARGET = false;
        private bool isThread = false;


        private const int _bytesCount = 1932; // посылка байтов для RF
        private const int _bytesACCount = 3400; // посылка байтов для AC спектр ЭС (1700) + (1700) обнаруженныые каналы
        private const int _bytesAC_Date_Count = 13600; // параметры обнаруживаемых Акустикой целей Fmin Fmax Fcentr, Fsize, Колличество привышений, колличество обн-х гармоник, азимут угол места.

        public const int PORT_1_5 = 30100;
        public const int PORT_2_4 = 30101;
        public const int PORT_5_2 = 30102;
        public const int PORT_5_8 = 30103;

        public const int PORT_AC = 30200;
        public const int PORT_AC_Date = 30201;

        private readonly IHubContext<UdpHub> _hub;

        public CancellationToken cancellationToken = default(CancellationToken);

        /// <summary>
        /// Массивы и параметры данных RF
        /// </summary>
        private float _header15;
        private float[] _sendSamples15 = new float[480];
        private float[] _sendSfarSamples15 = new float[480];
        private float isDetect15;
        /// <summary>
        /// Массивы и параметры данных RF
        /// </summary>
        private float _header24;
        private float[] _sendSamples24 = new float[480];
        private float[] _sendSfarSamples24 = new float[480];
        private float isDetect24;
        /// <summary>
        /// Массивы и параметры данных RF
        /// </summary>
        private float _header52;
        private float[] _sendSamples52 = new float[480];
        private float[] _sendSfarSamples52 = new float[480];
        private float isDetect52;
        /// <summary>
        /// Массивы и параметры данных RF
        /// </summary>
        private float _header58;
        private float[] _sendSamples58 = new float[480];
        private float[] _sendSfarSamples58 = new float[480];
        private float isDetect58;

        /// <summary>
        /// Массивы и параметры данных Акустики (Спектр и Каналы обнаружения)
        /// </summary>
        private float _headerAC;
        private float[] _sendSamplesAC = new float[425];
        private float[] _sendUPRSamplesAC = new float[425];

        /// <summary>
        /// Массивы и параметры данных Акустических параметров по целям
        /// </summary>
        private float[] _fMin = new float[425];
        private float[] _fMax = new float[425];
        private float[] _fCentr = new float[425];
        private float[] _fSize = new float[425];
        private float[] _numberOfTarget = new float[425];
        private float[] _numberGarmonik = new float[425];
        private float[] _azTarget = new float[425];
        private float[] _elvTarget = new float[425];

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

        private Socket _socketAC_Date;
        private EndPoint _ePAC_Date;


        private byte[] _buffer_recv;
        private byte[] _buffer_recvAC;
        private byte[] _buffer_recvAC_Date;

        private ArraySegment<byte> _buffer_recv_segment15;
        private ArraySegment<byte> _buffer_recv_segment24;
        private ArraySegment<byte> _buffer_recv_segment52;
        private ArraySegment<byte> _buffer_recv_segment58;
        private ArraySegment<byte> _buffer_recv_segmentAC;
        private ArraySegment<byte> _buffer_recv_segmentAC_Date;



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
            _buffer_recvAC_Date = new byte[_bytesAC_Date_Count];



            _buffer_recv_segment15 = new(_buffer_recv);
            _buffer_recv_segment24 = new(_buffer_recv);
            _buffer_recv_segment52 = new(_buffer_recv);
            _buffer_recv_segment58 = new(_buffer_recv);

            _buffer_recv_segmentAC = new(_buffer_recvAC);
            _buffer_recv_segmentAC_Date = new(_buffer_recvAC_Date);



            _eP15 = new IPEndPoint(IPAddress.Any, PORT_1_5);
            _eP24 = new IPEndPoint(IPAddress.Any, PORT_2_4);
            _eP52 = new IPEndPoint(IPAddress.Any, PORT_5_2);
            _eP58 = new IPEndPoint(IPAddress.Any, PORT_5_8);
            _ePAC = new IPEndPoint(IPAddress.Any, PORT_AC);
            _ePAC_Date = new IPEndPoint(IPAddress.Any, PORT_AC_Date);



            _socket15 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket24 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket52 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket58 = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socketAC = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socketAC_Date = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);



            _socket15.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket24.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket52.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket58.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socketAC.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socketAC_Date.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);



            _socket15.Bind(_eP15);
            _socket24.Bind(_eP24);
            _socket52.Bind(_eP52);
            _socket58.Bind(_eP58);
            _socketAC.Bind(_ePAC);
            _socketAC_Date.Bind(_ePAC_Date);


        }

        public void StartListeningPortsLoop()
        {
            // поток обработки акустических данных
            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult resAC_Date;

                while (true)
                {
                    resAC_Date = await _socketAC_Date.ReceiveMessageFromAsync(_buffer_recv_segmentAC_Date, SocketFlags.None, _ePAC_Date);                      
                    for (int j = 0; j < _fCentr.Length*4; j+=4)    
                    {                            
                        _fMin[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), j);
                        _fMax[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), j + _fCentr.Length*4);
                        _fCentr[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), (j + 2 * _fCentr.Length*4));
                        _fSize[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), j + 3 * _fCentr.Length*4);
                        _numberOfTarget[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), j + 4 * _fCentr.Length*4);
                        _numberGarmonik[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), j + 5 * _fCentr.Length * 4);
                        _azTarget[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), j + 6 * _fCentr.Length * 4);
                        _elvTarget[j / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC_Date.ToArray(), j + 7 * _fCentr.Length * 4);
                    }
                }
            });

            // поток обработки акустического спектра и отображения каналов обнаружения
            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult resAC;

                while (true)
                {
                    resAC = await _socketAC.ReceiveMessageFromAsync(_buffer_recv_segmentAC, SocketFlags.None, _ePAC);

                    for (int i = 0; i < (_buffer_recv_segmentAC.ToArray().Length) / 2; i += 4)
                    {
                        _sendSamplesAC[i / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC.ToArray(), i);
                    }

                    var m = _sendSamplesAC.Max();

                    for (int i = 0; i < _sendSamplesAC.Length; i++)
                    {
                        _sendSamplesAC[i] = _sendSamplesAC[i] / m;
                    }

                    for (int i = (_buffer_recv_segmentAC.ToArray().Length) / 2; i < _buffer_recv_segmentAC.ToArray().Length; i += 4)
                    {
                        _sendUPRSamplesAC[(i - (_buffer_recv_segmentAC.ToArray().Length / 2)) / 4] = BitConverter.ToSingle(_buffer_recv_segmentAC.ToArray(), i);
                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult res15;
               
                while (true)
                {
                    res15 = await _socket15.ReceiveMessageFromAsync(_buffer_recv_segment15, SocketFlags.None, _eP15);


                    // петрушка ХИДЭРА

                    _header15 = BitConverter.ToSingle(_buffer_recv_segment15.ToArray(), 0);

                    if (_header15 == 11100100)
                    {
                        for (int i = 4; i < _buffer_recv_segment24.ToArray().Length - 8; i += 4)
                        {
                            _sendSamples15[(i-4) / 4] = BitConverter.ToSingle(_buffer_recv_segment15.ToArray(), i);
                        }


                        float Z15 = BitConverter.ToSingle(_buffer_recv_segment15.ToArray(), _buffer_recv_segment15.ToArray().Length - 8);

                        for (int i = 0; i < _sendSfarSamples24.Length; i++)
                        {
                            _sendSfarSamples15[i] = Z15;
                        }

                         isDetect15 = BitConverter.ToSingle(_buffer_recv_segment15.ToArray(), _buffer_recv_segment15.ToArray().Length - 4);
                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {             
                SocketReceiveMessageFromResult res24;
               
                while (true)
                {
                    res24 = await _socket24.ReceiveMessageFromAsync(_buffer_recv_segment24, SocketFlags.None, _eP24);


                    _header24 = BitConverter.ToSingle(_buffer_recv_segment24.ToArray(), 0);

                    if (_header24 == 11100101)
                    {

                        for (int i = 4; i < _buffer_recv_segment24.ToArray().Length - 8; i += 4)
                        {
                            _sendSamples24[(i-4) / 4] = BitConverter.ToSingle(_buffer_recv_segment24.ToArray(), i);
                        }

                        float Z24 = BitConverter.ToSingle(_buffer_recv_segment24.ToArray(), _buffer_recv_segment24.ToArray().Length - 8);

                        for (int i = 0; i < _sendSfarSamples24.Length; i++)
                        {
                            _sendSfarSamples24[i] = Z24;
                        }

                        isDetect24 = BitConverter.ToSingle(_buffer_recv_segment24.ToArray(), _buffer_recv_segment24.ToArray().Length - 4);

                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {
                SocketReceiveMessageFromResult res52;

                while (true)
                {               
                    res52 = await _socket52.ReceiveMessageFromAsync(_buffer_recv_segment52, SocketFlags.None, _eP52);


                    _header52 = BitConverter.ToSingle(_buffer_recv_segment52.ToArray(), 0);

                    if (_header52 == 11100102)
                    {
                        for (int i = 4; i < _buffer_recv_segment52.ToArray().Length - 8; i += 4)
                        {
                            _sendSamples52[(i-4) / 4] = BitConverter.ToSingle(_buffer_recv_segment52.ToArray(), i);
                        }

                        float Z52 =  BitConverter.ToSingle(_buffer_recv_segment52.ToArray(), _buffer_recv_segment52.ToArray().Length - 8);

                        for (int i = 0; i < _sendSfarSamples24.Length; i++)
                        {
                            _sendSfarSamples52[i] = Z52;
                        }

                        isDetect52 = BitConverter.ToSingle(_buffer_recv_segment52.ToArray(), _buffer_recv_segment52.ToArray().Length - 4);
                    }
                }
            });

            _ = Task.Factory.StartNew(async () =>
            {             
                SocketReceiveMessageFromResult res58;

                while (true)
                {
                  
                    res58 = await _socket58.ReceiveMessageFromAsync(_buffer_recv_segment58, SocketFlags.None, _eP58);


                    _header58 = BitConverter.ToSingle(_buffer_recv_segment58.ToArray(), 0);

                    if (_header58 == 11100103)
                    {
                        for (int i = 4; i < _buffer_recv_segment58.ToArray().Length - 8; i += 4)
                        {
                            _sendSamples58[(i-4) / 4] = BitConverter.ToSingle(_buffer_recv_segment58.ToArray(), i);
                        }

                        float Z58 = BitConverter.ToSingle(_buffer_recv_segment58.ToArray(), _buffer_recv_segment58.ToArray().Length - 8);

                        for (int i = 0; i < _sendSfarSamples24.Length; i++)
                        {
                            _sendSfarSamples58[i] = Z58;
                        }

                        isDetect58 = BitConverter.ToSingle(_buffer_recv_segment58.ToArray(), _buffer_recv_segment58.ToArray().Length - 4);

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
                }

                await _hub.Clients.All.SendAsync(
                        "addChartData",
                        new PointRF(_sendSamples15, _sendSfarSamples15, _sendSamples24, _sendSfarSamples24,
                                    _sendSamples52, _sendSfarSamples52, _sendSamples58, _sendSfarSamples58,
                                    _sendUPRSamplesAC, _sendSamplesAC, 360-_azAnScan, _azTarget, _fMin),
                        cancellationToken: stoppingToken
                    );


         
                if (isAutoMode && !isThread)
                {
                    Task.Factory.StartNew(() => AutoMode(), cancellationToken).GetAwaiter();              
                }

                
                await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);
             }
        }

        public async Task AutoMode()
        {
            if (isAutoMode)
            {
                isThread = true;

                for (int i = 0; i < _numberGarmonik.Length; i++)
                {
                    if (_numberGarmonik[i] >= 2 || isDetect15 > 0 || isDetect24 > 0 || isDetect52 > 0 || isDetect58 > 0)
                    {
                        isTARGET = true;
                    }
                }
                if (isTARGET)
                {
                    isTARGET = false;

                    await UDPSendServer.SendCommand(new SendDate(new DataDevice(new string[] { "true", "false", "false", "false" })));

                    await Task.Delay(TimeSpan.FromSeconds(15));

                    await UDPSendServer.SendCommand(new SendDate(new DataDevice(new string[] { "true", "true", "true", "true" })));

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                isThread = false;
            }
        }
    }
}
