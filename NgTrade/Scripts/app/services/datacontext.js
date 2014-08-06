/* dataservice: data access and model management layer 
 * relies on Angular injector to provide:
 *     $timeout - Angular equivalent of 'setTimeout'
 *     breeze - the Breeze.Angular service (which is breeze itself)
 *     logger - the application's logging facility
 */
(function () {

    angular.module('main').factory('datacontext',
    ['$timeout', 'breeze', dataservice]);

    function dataservice($timeout, breeze) {

        var serviceName = 'wapi/data';

        var manager = new breeze.EntityManager(serviceName);
        manager.enableSaveQueuing(true);

        var service = {
            addPropertyChangeHandler: addPropertyChangeHandler,
            getDailylist: getDailylist,
            getStock: getStock,
            getStockHistoryUtc: getStockHistoryUtc
        };
        return service;

        function addPropertyChangeHandler(handler) {
            // call handler when an entity property of any entity changes
            return manager.entityChanged.subscribe(function (changeArgs) {
                var action = changeArgs.entityAction;
                if (action === breeze.EntityAction.PropertyChange) {
                    handler(changeArgs);
                }
            });
        }

        function getDailylist() {
            var query = breeze.EntityQuery
                .from("Dailypricelists");

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                throw error;
            }
        }

        function getStock(symbol) {
            var query = breeze.EntityQuery
                .from("Dailypricelists")
                .where("Stock", "==", symbol);

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                throw error;
            }
        }

        function getStockHistoryUtc(symbol, fromDate, toDate) {
            var query = breeze.EntityQuery
                .from("QuoteUtcDate")
                .where("Symbol", "==", symbol);

            if (fromDate || toDate) {
                var fromDateInt = parseInt(fromDate);
                var toDateInt = parseInt(toDate);

                query = query
                    .where("DateTimeUtc", "gt", fromDateInt)
                    .where("DateTimeUtc", "lt", toDateInt);
            }
            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                throw error;
            }
        }
    }

})();