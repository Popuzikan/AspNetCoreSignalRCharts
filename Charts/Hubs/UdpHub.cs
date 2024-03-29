﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Charts.StructTypes;
using Charts.Services;
using System.ComponentModel;

namespace Charts.Hubs
{
    public class UdpHub : Hub
    {
        public const string Url = "/udp";

        public SendDate SendInfo { get; set; }

       // public UDPSendServer sendServer = new UDPSendServer();

        public async Task Send(DataDevice user)
        {

            if (user.PmStack.Length.Equals(5))
            {
                SendInfo = new SendDate(user);

               await UDPSendServer.SendCommand(SendInfo);

                UDPServer.isAutoMode = Convert.ToBoolean(user.PmStack[4]);
            }

            //await sendServer.SendCommand(SendInfo);



            await Task.Delay(1);
        }
    }
}
