using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Charts.Hubs
{
    public class UdpHub : Hub
    {
        public const string Url = "/udp";

        public DataDevice Device { get; set; }

        public async Task Send(DataDevice user)
        {
            

            await Task.Delay(1);
        }

    }





    public struct DataDevice
    {
        //public UInt16 StructHeader;
        //public Int16 Azimut_flag;
        //public Int16 Azimut;
        //public Int16 UgolMesta_flag;
        //public UInt16 UgolMesta;

        public bool Pm1;
        //public bool _pm2;
        //public bool _pm3;
        //public bool _pm4;

        public DataDevice(bool Pm1)
        {
            //StructHeader = 0xAA11;
            //Azimut_flag = 1;
            //Azimut = 0;
            //UgolMesta_flag = 0;
            //UgolMesta = 0;

            this.Pm1 = Pm1;
            //_pm2 = pm2;
            //_pm3 = pm3;
            //_pm4 = pm4;
        }

        /// <summary>
        /// false - включить Jammer
        /// true - отключить Jammer
        /// </summary>
        /// 
    }




}
