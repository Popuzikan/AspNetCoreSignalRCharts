//using System.Globalization;
//using System.Security.Cryptography;
//using Charts.Hubs;
//using Microsoft.AspNetCore.SignalR;
//using Charts.Interfaces;
//using Charts.ServicesAbstract;

//namespace Charts.Services;

//public class ChartValueGenerator : BackgroundService
//{
//    private readonly IHubContext<ChartHub> _hub;
//    private readonly Buffer<Point> _data;

//    private readonly UDPServer _udpServer;

//    private  PointRF _pointRF;

//    public ChartValueGenerator(IHubContext<ChartHub> hub, Buffer<Point> data, ITransmiter transmiter, UDPServer uDPServer)
//    {
//        _hub = hub;
//        _data = data;

//        _udpServer = uDPServer;

//        _udpServer.Initialize();
//        _udpServer.StartListeningPortsLoop();
//    }
    
//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {

//            await _hub.Clients.All.SendAsync(
//                "addChartData",
//                _pointRF,
//                cancellationToken: stoppingToken
//            );


//            //await _hub.Clients.All.SendAsync(
//            //    "addChartData",
//            //    _data.AddNewRandomPoint(), 
//            //    cancellationToken: stoppingToken
//            //);

//            await Task.Delay(TimeSpan.FromSeconds(0.2), stoppingToken);
//        }
//    }


//    public void SendRFSamples(object obj, TransmiteDateArgs args)
//    {
//        _pointRF = args.Samples; 
//    }



//}