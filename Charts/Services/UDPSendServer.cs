using System.Net.Sockets;
using Charts.StructTypes;

namespace Charts.Services
{
    public class UDPSendServer : BackgroundService
    {
        public UdpClient udpClient = null;


        public UDPSendServer()
        {
            udpClient = new UdpClient(31010);

            try
            {
                udpClient.Connect("255.255.255.255", 31010);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public async Task SendCommand(SendDate send)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryWriter binaryWriter = new BinaryWriter(stream);

                binaryWriter.Write(send.StructHeader);
                binaryWriter.Write(send.Azimut_flag);
                binaryWriter.Write(send.Azimut);
                binaryWriter.Write(send.UgolMesta_flag);
                binaryWriter.Write(send.UgolMesta);
                binaryWriter.Write(send.isPmActive[0]);
                binaryWriter.Write(send.isPmActive[1]);
                binaryWriter.Write(send.isPmActive[2]);
                binaryWriter.Write(send.isPmActive[3]);

                binaryWriter.Flush();

                byte[] str = stream.ToArray();

                int bytes = await udpClient.SendAsync(str, str.Length);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
                return Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);
        }
    }
}
