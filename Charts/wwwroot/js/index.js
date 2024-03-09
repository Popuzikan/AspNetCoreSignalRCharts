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

    radarForm1.extendTraces([0, data.y - 5, data.y + 5, 0,
        data.z[1] - 3, data.z[1] + 3, 0,
        data.z[2] - 3, data.z[2] + 3, 0,
        data.z[3] - 3, data.z[3] + 3, 0,
        data.z[4] - 3, data.z[4] + 3, 0,
        data.z[5] - 3, data.z[5] + 3, 0,
        data.z[6] - 3, data.z[6] + 3, 0,
        data.z[7] - 3, data.z[7] + 3, 0,
        data.z[8] - 3, data.z[8] + 3, 0,
        data.z[9] - 3, data.z[9] + 3, 0]);

});

signalHubConnection.start();

function pad(num, size) {
    return ('000000000' + num).substr(-size);
}

