////var waveFormGraph = new WaveForm(document.getElementById('graphWaveForm'));
////var moduleWaveFormGraph = new WaveForm(document.getElementById('moduleWaveForm'));

var corrGraph = new CorrelationGraph(document.getElementById('corrWaveForm'));

const signalHubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/signal")
    .build();

signalHubConnection.on("CorrelationDetected", (data) => corrGraph.extendTraces([[data.correlationMax], [data.threshold]]));
signalHubConnection.start();