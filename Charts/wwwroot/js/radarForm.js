function RadarForm(element) {
    var _element = element;
    var _tracesCount;
    var _tracesIndexes;
    var _xValues;

    init(_element);

    function init(element) {
        var SamplingRate = 1;
        _xValues = Array(SamplingRate).fill().map((_, i) => i);

        layout = {
           
            showlegend: false,

            angularaxis: { color: '#ffffff' },
            plot_bgcolor: 'rgba(0, 0, 0, 0)',
            paper_bgcolor: 'rgba(0, 0, 0, 0)',
            height: 300,
            width: 300,

            margin: {
                l: 0,
                r: 0,
                b: 0,
                t: 2,
                pad: 0
            }
        }
       

        Plotly.react(element, [], layout, { responsive: true });
    }

    function extendTraces(traces) {
        if (_tracesCount != traces.length) {
            resetTraces(traces.length);
        }

        var extend = {};
        extend.y = traces;

        Plotly.extendTraces(_element, extend, _tracesIndexes, _xValues.length);
    }

    function resetTraces(newTracesCount) {
        console.log(newTracesCount);
        _element.data = [];
        _tracesCount = newTracesCount;

        _tracesIndexes = Array(_tracesCount)
            .fill()
            .map(function (_, i) {
                return i
            });

        var traces = Array(_tracesCount).fill().map(function (_, i) {
            return {
                r: [0, 2 , 2, 0],
                theta: [0,55,35 ,0],
                mode: 'lines',
               // name: 'Канал ' + (i + 1),
                type: "scatterpolar",
                fill: "toself",
                line: {
                    color: 'red',
                    width : 1
                }
            };
        })
        Plotly.addTraces(_element, traces);
    }

    return {
        extendTraces: extendTraces
    }
}