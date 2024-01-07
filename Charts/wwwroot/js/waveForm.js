function WaveForm(element) {
    var _element = element;
    var _tracesCount;
    var _tracesIndexes;
    var _xValues;

    init(_element);

    function init(element) {
        var secondsAccumulationTime = 0.02;
        var SamplingRate = 350;
        _xValues = Array(100).fill().map((_, i) => i / SamplingRate * 1000);

        var layout = {
            title: 'Развертка по времени',
            yaxis: {
                //range: [MIN_Y, MAX_Y],
                autorange: true,
                title: {
                    text: 'Амплитуда',
                }
            },
            xaxis: {
                title: {
                    text: 'Время, мс'
                },
            }
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
                name: 'Канал ' + (i + 1)
            };
        })
        Plotly.addTraces(_element, traces);
    }

    return {
        extendTraces: extendTraces
    }
}