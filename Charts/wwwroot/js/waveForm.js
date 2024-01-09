function WaveForm(element) {
    var _element = element;
    var _tracesCount;
    var _tracesIndexes;
    var _xValues;

    init(_element);

    function init(element) {
        var secondsAccumulationTime = 0.02;
        var SamplingRate = 480;
        _xValues = Array(SamplingRate).fill().map((_, i) => i);

        var layout = {
            yaxis: {
                range: [0, 300],
                //autorange: true,
                //title: {
                //    text: 'Амплитуда',
                showgrid: false,
                zeroline: false,
                showline: false,
                autotick: true,
                ticks: '',
                showticklabels: false
                //}
            },
            xaxis: {
                
                //autorange: true,
                //title: {
                //    text: 'Амплитуда',
                showgrid: false,
                zeroline: false,
                showline: false,
                autotick: true,
                ticks: '',
                showticklabels: false
                //}
            },

            margin: {
                l: 0,
                r: 0,
                b: 0,
                t: 2,
                pad: 0
            },

            height: 120,
            width: 750,
            plot_bgcolor: 'rgba(0, 0, 0, 0)',
            paper_bgcolor: 'rgba(0, 0, 0, 0)',

            
            //xaxis: {
            //    title: {
            //        text: ''
            //    },
            //}         
        };

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
                y: [],
                x: _xValues,
                mode: 'lines',
               // name: 'Канал ' + (i + 1),
                fill: 'tonexty',
                //type: 'bar',
                line: {
                    shape: 'spline',
                    color: 'green',
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