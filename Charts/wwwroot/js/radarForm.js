
function RadarForm(element) {
    var _element = element;
    var _tracesCount;
    var _tracesIndexes;
    var _xValues;

    var counT = 0;
    var Z = 0;
    var colores = ['red','green']

    init(_element);

    function init(element) {
        var SamplingRate = 1;
        _xValues = Array(SamplingRate).fill().map((_, i) => i);

        layout = {

            polar: {
                bgcolor: 'rgba(0, 0, 0, 0)',

                radialaxis: {
                    visible: false,
                    //color:'red',
                    range: [0, 2],
                    showticklabels: false,
                    //angle: 0,
                    //tickangle: 10,
                    tickwidth: 10,            
                    tickfont: {
                        size: 18
                    },
                    gridcolor: "yellow",
                    gridwidth: 3
                },
                angularaxis: {
                    tickfont: {
                        size: 18
                    },      
                    tickwidth: 10,
                    linewidth: 3,
                    color: 'yellow',
                    gridcolor: "yellow",
                    gridwidth: 3
                }
            },
                
            //showlegend: false,

            //angularaxis: { color: '#ffffff' },
            plot_bgcolor: 'rgba(0, 0, 0, 0)',
            paper_bgcolor: 'rgba(0, 0, 0, 0)',
            height: 800,
            width: 800,

            margin: {
                l: 54,
                r: 54,
                b: 0,
                t: 0,
                pad: 0
            }
        }
       

        Plotly.react(element, [], layout, { responsive: true });
    }

    function extendTraces(traces) {
        if (_tracesCount != traces.length) {
            resetTraces(2, traces);
        }

        var extend = {};
        extend.y = traces;

        Plotly.extendTraces(_element, extend, _tracesIndexes, _xValues.length);
    }

    function resetTraces(newTracesCount, dataI) {
        console.log(newTracesCount);
        _element.data = [];
        _tracesCount = newTracesCount;

        counT = 0;

        _tracesIndexes = Array(_tracesCount)
            .fill()
            .map(function (_, i) {
                return i
            });

        var traces = Array(_tracesCount).fill().map(function (_, i) {
            return {
                r: [0, 2, 2,
                    0, 2, 2,
                    0, 2, 2,
                    0, 2, 2,
                    0, 2, 2,
                    0, 2, 2,
                    0, 2, 2,
                    0, 2, 2,
                    0, 2, 2,
                    0, 2, 2, 0],
                 theta: dataI,
              
                mode: 'lines',

                // name: 'Канал ' + (i + 1),
                type: "scatterpolar",
                fill: "toself",
                fillcolor: 'green',
                opacity: 0.5,
                line: {
                    color: 'green',
                    width: 2
                }, 
            };       
        })
        Plotly.addTraces(_element, traces);
    }

    return {
        extendTraces: extendTraces
    }
}