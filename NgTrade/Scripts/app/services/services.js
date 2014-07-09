var app = angular.module('app', []);
app.value('$', $);
app.service('stockTickerData', [
    '$', '$rootScope', function($, $rootScope) {
        var connection;
        var proxy;

        var initializeClient = function() {
            connection = $.hubConnection();
            proxy = connection.createHubProxy('stockTicker');

            configureProxyClientFunctions();

            start();
        };

        var configureProxyClientFunctions = function() {
            proxy.on('marketOpened', function() {
                $rootScope.$emit('marketOpened', true);
            });

            proxy.on('marketClosed', function() {
                $rootScope.$emit('marketClosed', true);
            });

            proxy.on('marketReset', function() {
                initializeStockMarket();
            });

            proxy.on('updateStockPrice', function(stock) {
                $rootScope.$emit('updateStock', stock);
            });
        };

        var initializeStockMarket = function() {
            proxy.invoke('getAllStocks').done(function(data) {
                $rootScope.$emit('setStocks', data);
            }).pipe(function() {
                proxy.invoke('getMarketState').done(function(state) {
                    if (state == 'Open') {
                        $rootScope.$emit('marketOpened', true);
                    } else {
                        $rootScope.$emit('marketClosed', true);
                    }
                });
            });
        };

        var start = function() {
            connection.start().pipe(function() {
                initializeStockMarket();
            });
        };

        var openMarket = function() {
            proxy.invoke('openMarket');
        };

        var closeMarket = function() {
            proxy.invoke('closeMarket');
        };

        var reset = function() {
            proxy.invoke('reset');
        };

        return {
            initializeClient: initializeClient,
            openMarket: openMarket,
            closeMarket: closeMarket,
            reset: reset
        };
    }
]);