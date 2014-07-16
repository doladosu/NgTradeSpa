/* TodoController - the controller for the "todo view" 
 * relies on Angular injector to provide:
 *     $q - promises manager
 *     $timeout - Angular equivalent of 'setTimeout'
 *     dataservice - the application data access service
 *     logger - the application's logging facility
 */
(function() {

    angular.module('main').controller('DailyListController',
    ['$q', '$timeout', 'datacontext', '$scope', controller]);

    function controller($q, $timeout, datacontext, $scope) {
        // The controller's API to which the view binds
        var vm = this;
        $scope.dailyPrices = [];

        vm.getDailylist = getDailylist;

        getDailylist();

        // Listen for property change of ANY entity so we can (optionally) save
        //datacontext.addPropertyChangeHandler(propertyChanged);

        /* Implementation */

        function getDailylist() {
            //editEnd();
            // wait for Ng binding to set 'includeArchived' flag, then proceed
            $timeout(getDailylistImpl, 0);

            function getDailylistImpl() {
                datacontext.getDailylist()
                    .then(querySucceeded);
            }

            function querySucceeded(data) {
                vm.items = data.results;
                $scope.dailyPrices = data.results;
            }
        };

    //    $scope.filterOptions = {
    //        filterText: "",
    //        useExternalFilter: true
    //    };
    //    $scope.totalServerItems = 0;
    //    $scope.pagingOptions = {
    //        pageSizes: [5, 10, 20],
    //        pageSize: 5,
    //        currentPage: 1
    //    };

    //    $scope.setPagingData = function (data, page, pageSize) {
    //        var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
    //        $scope.myData = pagedData;
    //        $scope.totalServerItems = data.length;
    //        if (!$scope.$$phase) {
    //            $scope.$apply();
    //        }
    //    };

    //    $scope.getPagedDataAsync = function (pageSize, page, searchText) {
    //        setTimeout(function () {
    //            var data;
    //            if (searchText) {
    //                var ft = searchText.toLowerCase();
    //                $timeout(getDailylistNgGrid, 0);

    //                function getDailylistNgGrid() {
    //                    datacontext.getDailylist()
    //                        .then(querySucceeded);
    //                }

    //                function querySucceeded(dataRe) {
    //                    data = dataRe.results.filter(function (item) {
    //                        return JSON.stringify(item).toLowerCase().indexOf(ft) != -1;
    //                    });
    //                    $scope.setPagingData(data, page, pageSize);
    //                }
    //            } else {
    //                $timeout(getDailylistNgGridd, 0);

    //                function getDailylistNgGridd() {
    //                    datacontext.getDailylist()
    //                        .then(querySucceededd);
    //                }
                    
    //                function querySucceededd(dataRe) {
    //                    $scope.usersList = [];
    //                    angular.forEach(dataRe, function (userData) {
    //                        $scope.usersList.push(userData);
    //                    });
    //                    $scope.setPagingData(usersList, page, pageSize);
    //                }
    //            }
    //        }, 0);
    //    };

    //    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

    //    $scope.$watch('pagingOptions', function (newVal, oldVal) {
    //        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
    //            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
    //        }
    //    }, true);
    //    $scope.$watch('filterOptions', function (newVal, oldVal) {
    //        if (newVal !== oldVal) {
    //            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
    //        }
    //    }, true);

    //    $scope.gridOptions = {
    //        data: 'myData',
    //        enablePaging: true,
    //        showFooter: true,
    //        totalServerItems: 'totalServerItems',
    //        pagingOptions: $scope.pagingOptions,
    //        filterOptions: $scope.filterOptions
    //    };
    }

})();