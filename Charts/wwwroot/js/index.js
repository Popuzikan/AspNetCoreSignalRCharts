const data = JSON.parse(document.getElementById('data').innerHTML);


var corrGraph = new CorrelationGraph(document.getElementById('corrWaveForm'));

const signalHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(data.url)
    .configureLogging(signalR.LogLevel.Information)
    .build();

signalHubConnection.on("addChartData", (data) => corrGraph.extendTraces([[data], [data]]));

signalHubConnection.start();

























//const data = JSON.parse(document.getElementById('data').innerHTML);
//const ctx = document.getElementById('myChart').getContext('2d');


//const myChart = new Chart(ctx, {
//    type: 'line',
//    data: data,
//    options : {
//        scales: {
//            y : {
//                suggestedMax: 100,
//                suggestedMin: 1
//            }
//        }
//    }
//});

//const connection = new signalR.HubConnectionBuilder()
//    .withUrl(data.url)
//    .configureLogging(signalR.LogLevel.Information)
//    .build();

//async function start() {
//    try {
//        await connection.start();
//        console.log("SignalR Connected.");
//    } catch (err) {
//        console.log(err);
//        setTimeout(start, 5000);
//    }
//}

//connection.onclose(async () => {
//    await start();
//});

//connection.on("addChartData", function (point) {
 
//    myChart.data.datasets.forEach((dataset) => {

//        dataset.data.push([point.value]);
//        myChart.data.labels.push([point.labels]);
//    });

//    myChart.update();

//    if (myChart.data.labels.length > data.limit) {
//        myChart.data.labels.splice(0, 350);
//        myChart.data.datasets.forEach((dataset) => {
//            dataset.data.splice(0, 350);
//        });
//        myChart.update();
//    }
//});

// Start the connection.
/*start().then(() => {});*/