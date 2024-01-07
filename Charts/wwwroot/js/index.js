const data = JSON.parse(document.getElementById('data').innerHTML);


var corrGraph = new WaveForm(document.getElementById('corrWaveForm'));

const signalHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(data.url)
    .configureLogging(signalR.LogLevel.Information)
    .build();

signalHubConnection.on("addChartData", (data) => {
    console.log(data);
    corrGraph.extendTraces([data.x]);
});

signalHubConnection.start();

function pad(num, size) {
    return ('000000000' + num).substr(-size);
}

