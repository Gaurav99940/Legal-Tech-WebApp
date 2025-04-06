(function (window, document, $) {
    'use strict';

    var analysisgraph = document.getElementById('analysis-graph');
    if (analysisgraph != null) {
        var settings = {
            "url": "/API/EffortAnalysis",
            "method": "POST",
            "dataType": "json",
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {

            var ctx = $("#analysis-graph");
            var chartOptions = {
                responsive: true,
                maintainAspectRatio: false,
                legend: {
                    display: false
                },
                hover: {
                    mode: 'label'
                },
                scales: {
                    xAxes: [{
                        display: true,
                        gridLines: {
                            color: "#f3f3f3",
                            drawTicks: false,
                        }
                    }],
                    yAxes: [{
                        display: true,
                        gridLines: {
                            color: "#f3f3f3",
                            drawTicks: false,
                        },
                        ticks: {
                            display: true,
                            maxTicksLimit: 5
                        }
                    }]
                },
                title: {
                    display: false,
                }
            };
            var chartData = {
                labels: response.name,
                datasets: [{
                    label: "Effort Proposed",
                    data: response.effortproposed,
                    fill: false,
                    borderColor: "#00A5A8",
                    pointBorderColor: "#00A5A8",
                    pointBackgroundColor: "#FFF",
                    pointBorderWidth: 2,
                    pointHoverBorderWidth: 2,
                    pointRadius: 4,
                }, {
                    label: "Effor Consumed",
                    data: response.efforconsumed,
                    fill: false,
                    borderColor: "#FF4961",
                    pointBorderColor: "#FF4961",
                    pointBackgroundColor: "#FFF",
                    pointBorderWidth: 2,
                    pointHoverBorderWidth: 2,
                    pointRadius: 4,
                }]
            };
            var config = {
                type: 'line',
                options: chartOptions,
                data: chartData
            };
            var lineChart = new Chart(ctx, config);
        });
    }




    var categorydata = document.getElementById('category-data');
    if (categorydata != null) {
        var settings = {
            "url": "/API/EffortAnalysisBar",
            "method": "POST",
            "dataType": "json",
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {

            var categorydata = c3.generate({
                bindto: '#category-data',
                size: { height: 400 },
                color: {
                    pattern: ['#673AB7', '#E91E63']
                },

                // Create the data table.
                data: {
                    x: 'Analysis',
                    columns: [
                        response.name,
                        response.effortproposed,
                        response.efforconsumed,
                    ],

                    type: 'bar'
                },
                axis: {
                    x: {
                        type: 'category' // this needed to load string x value
                    }
                },
                grid: {
                    y: {
                        show: true
                    }
                },
            });
        });
    }



    var divmonthwiseanalysis = document.getElementById('monthwiseanalysis');
    if (divmonthwiseanalysis != null) {
        var settings = {
            "url": "/API/GetLastSixMonthInitiatives",
            "method": "POST",
            "dataType": "json",
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {
            Morris.Bar({
                element: 'monthwiseanalysis',
                data: response,
                xkey: 'monthyear',
                ykeys: ['initiativecount'],
                labels: ['Initiative Count'],
                barGap: 4,
                barSizeRatio: .6,
                gridTextColor: '#bfbfbf',
                gridLineColor: '#e3e3e3',
                numLines: 5,
                gridtextSize: 10,
                resize: true,
                barColors: ['#37bc9b'],
                hideHover: 'auto',
            });
        });

    }

    var dashboarddonutchart = document.getElementById('dashboarddonutchart');
    if (dashboarddonutchart != null) {
        var settings = {
            "url": "/API/GetDashboardDonutData",
            "method": "POST",
            "dataType": "json",
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {
            Morris.Donut({
                element: 'dashboarddonutchart',
                data: response.donutdata,
                resize: true,
                colors: response.donutcolor
            });
        });
    }
})(window, document, jQuery);