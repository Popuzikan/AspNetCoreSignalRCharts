const data = JSON.parse(document.getElementById('data').innerHTML);


var corrGraph1 = new WaveForm(document.getElementById('corrWaveForm1'));

var corrGraph2 = new WaveForm(document.getElementById('corrWaveForm2'));

var corrGraph3 = new WaveForm(document.getElementById('corrWaveForm3'));

var corrGraph4 = new WaveForm(document.getElementById('corrWaveForm4'));

var corrGraph5 = new AcousticWaveForm(document.getElementById('corrWaveForm5'));


const signalHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(data.url)
    .configureLogging(signalR.LogLevel.Information)
    .build();

signalHubConnection.on("addChartData", (data) => {
    console.log(data);

    corrGraph1.extendTraces([data.x]);

    corrGraph2.extendTraces([data.x]);

    corrGraph3.extendTraces([data.x]);

    corrGraph4.extendTraces([data.x]);

    corrGraph5.extendTraces([data.x]);

});

signalHubConnection.start();

function pad(num, size) {
    return ('000000000' + num).substr(-size);
}

