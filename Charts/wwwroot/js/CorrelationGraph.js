function CorrelationGraph(element) {
    var _element = element;
    var _tracesIndexes = [0, 1]

    init(_element);

    function init(element) {
        var layout = {
            title: 'Выход разностно-временного устройства',
            yaxis: {
                title: {
                    text: 'Амплитуда'
                }
            }
        };

        var data = [
            {
                y: [],
                x: [],
                mode: 'lines',
                name: 'Max'
            },
            {
                y: [],
                x: [],
                mode: 'lines',
                name: 'Порог'
            }];

        Plotly.react(element, data, layout, { responsive: true });
    }

    function extendTraces(traces) {
        var extend = {};
        extend.y = [traces.y.y]; //это я дописал

        extend.x = [traces.x.x]; // это тоже

        Plotly.extendTraces(_element, extend, _tracesIndexes, 10);
    }

    return {
        extendTraces: extendTraces
    }
}