const data = JSON.parse(document.getElementById('data').innerHTML);


var corrGraph1 = new WaveForm(document.getElementById('corrWaveForm1'));

var corrGraph2 = new WaveForm(document.getElementById('corrWaveForm2'));

var corrGraph3 = new WaveForm(document.getElementById('corrWaveForm3'));

var corrGraph4 = new WaveForm(document.getElementById('corrWaveForm4'));

var corrGraph5 = new AcousticWaveForm(document.getElementById('corrWaveForm5'));

var radarForm1 = new RadarForm(document.getElementById('radarForm'));


const signalHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(data.url)
    .configureLogging(signalR.LogLevel.Information)
    .build();

signalHubConnection.on("addChartData", (data) => {

    corrGraph1.extendTraces([data.a, data.b]); // 1.5 RF

    corrGraph2.extendTraces([data.c, data.d]); // 2.4 RF

    corrGraph3.extendTraces([data.i, data.f]); // 5.2 RF

    corrGraph4.extendTraces([data.g, data.h]); // 5.8 RF

    corrGraph5.extendTraces([data.k, data.l]); // acoustic

   // radarForm1.extendTraces([0, data.y - 5, data.y + 5, 0, data.z - 15, data.z + 15, 0]);

    radarForm1.extendTraces([0, data.y - 5, data.y + 5,  0, data.z - 15, data.z + 15, 0]);

});

signalHubConnection.start();

function pad(num, size) {
    return ('000000000' + num).substr(-size);
}

