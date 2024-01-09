using Charts.Hubs;
using Charts.Services;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// our data source, could be a database
builder.Services.AddSingleton(_ =>
{
    var buffer = new Buffer<PointRF>(1);
    // start with something that can grow
    //for (var i = 0; i < 350; i++)
    //    buffer.AddNewRandomPoint();

    return buffer;
});

//builder.Services.AddHostedService<ChartValueGenerator>();
builder.Services.AddHostedService<UDPServer>();


var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();
app.MapHub<UdpHub>(UdpHub.Url);

app.Run();