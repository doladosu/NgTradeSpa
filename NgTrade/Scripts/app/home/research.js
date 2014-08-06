(function () {
    angular.module('main').controller('ResearchController',
    ['$q', '$timeout', 'datacontext', '$scope', '$location', '$window', '$routeParams', controller]);

    function controller($q, $timeout, datacontext, $scope, $location, $window, $routeParams) {
        $scope.$root.title = 'NgTradeOnline';
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
        
        var stockToResearch = ($routeParams.stock) ? $routeParams.stock : '';

        if (stockToResearch != "") {
            getStock(stockToResearch);
            getStockHistory(stockToResearch);

            function getStock(symbol) {
                datacontext.getStock(symbol)
                    .then(function(stock) {
                        $scope.stock = stock.results[0];
                    });
            }

            function getStockHistory(symbol) {
                datacontext.getStockHistoryUtc(symbol, "", "")
                    .then(function(dataR) {
                        var formatData = [];
                        angular.forEach(dataR.results, function(value, key) {
                            var dayData = [value.DateTimeUtc, value.Open, value.High, value.Low, value.Close];
                            this.push(dayData);
                        }, formatData);
                        var data = formatData;

                        // create the chart
                        $('#highchartcontainer').highcharts('StockChart', {
                            chart: {
                                type: 'candlestick',
                                zoomType: 'x'
                            },

                            navigator: {
                                adaptToUpdatedData: false,
                                series: {
                                    data: data
                                }
                            },

                            scrollbar: {
                                liveRedraw: false
                            },

                            title: {
                                text: stockToResearch + ' chart history'
                            },

                            rangeSelector: {
                                buttons: [
                                    {
                                        type: 'day',
                                        count: 1,
                                        text: '1d'
                                    }, {
                                        type: 'month',
                                        count: 1,
                                        text: '1m'
                                    }, {
                                        type: 'month',
                                        count: 3,
                                        text: '3m'
                                    }
                                ],
                                inputEnabled: false, // it supports only days
                                selected: 4 // all
                            },

                            xAxis: {
                                events: {
                                    afterSetExtremes: afterSetExtremes
                                },
                                minRange: 3600 * 1000 // one hour
                            },

                            yAxis: {
                                floor: 0
                            },

                            series: [
                                {
                                    data: data,
                                    dataGrouping: {
                                        enabled: false
                                    }
                                }
                            ]
                        });
                    });
            }

            function afterSetExtremes(e) {
                var currentExtremes = this.getExtremes(),
                    range = e.max - e.min,
                    chart = $('#highchartcontainer').highcharts();

                chart.showLoading('Loading data from server...');
                datacontext.getStockHistoryUtc(stockToResearch, Math.round(e.min).toString(), Math.round(e.max).toString())
                    .then(function(dataR) {
                        var formatData = [];
                        angular.forEach(dataR.results, function(value, key) {
                            var dayData = [value.DateTimeUtc, value.Open, value.High, value.Low, value.Close];
                            this.push(dayData);
                        }, formatData);
                        var data = formatData;
                        chart.series[0].setData(data);
                        chart.hideLoading();
                    });
            }
        }
    }
})();