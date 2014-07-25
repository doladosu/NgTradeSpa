(function () {
    angular.module('main').controller('AccountController',
    ['$q', '$timeout', 'datacontext', '$scope', '$location', '$window', controller]);

    function controller($q, $timeout, datacontext, $scope, $location, $window) {
        $scope.$root.title = 'NgTradeOnline';
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
        $scope.columnDefs = [
            { field: 'Stock', displayName: 'Stock' },
            { field: 'Open', displayName: 'Open' },
            { field: 'High', displayName: 'High' },
            { field: 'Low', displayName: 'Low' },
            { field: 'Close', displayName: 'Close' },
            { field: 'Change', displayName: 'Change', cellTemplate: '<div ng-class="{green: row.getProperty(col.field) > 0, red: row.getProperty(col.field) < 0}"><div class="ngCellText">{{row.getProperty(col.field)}}</div></div>' },
            { field: 'Volume', displayName: 'Volume' }
        ];

        $scope.selectAll = function () {
            $scope.filterOptions.dayChange = 0;
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.dayChange);
        };

        $scope.selectGainers = function () {
            $scope.filterOptions.dayChange = 1;
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.dayChange);
        };

        $scope.selectLosers = function () {
            $scope.filterOptions.dayChange = 2;
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText, $scope.filterOptions.dayChange);
        };

        $scope.filterOptions = {
            filterText: "",
            dayChange: 0,
            useExternalFilter: true
        };
        $scope.totalServerItems = 0;
        $scope.pagingOptions = {
            pageSizes: [25, 50, 100],
            pageSize: 25,
            currentPage: 1
        };

        $scope.setPagingData = function (data, page, pageSize) {
            var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
            $scope.dayList = pagedData;
            $scope.totalServerItems = data.length;
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };
        $scope.getPagedDataAsync = function (pageSize, page, searchText) {
            $scope.searchText = searchText;
            $scope.page = page;
            $scope.pageSize = pageSize;

            $timeout(getDailylistNgGrid, 0);

            function getDailylistNgGrid() {
                datacontext.getDailylist()
                    .then(querySucceeded);
            }
        };

        function querySucceeded(dataRe) {
            var data;
            if ($scope.searchText) {
                var ft = $scope.searchText.toLowerCase();
                data = dataRe.results.filter(function (item) {
                    return JSON.stringify(item.Stock).toLowerCase().indexOf(ft) != -1;
                });
                $scope.setPagingData(data, $scope.page, $scope.pageSize);
            }
            else if ($scope.filterOptions.dayChange) {
                if ($scope.filterOptions.dayChange == 1) {
                    data = dataRe.results.filter(function (item) {
                        return parseFloat(JSON.stringify(item.Change)) > 0;
                    });
                }
                else if ($scope.filterOptions.dayChange == 2) {
                    data = dataRe.results.filter(function (item) {
                        return parseFloat(JSON.stringify(item.Change)) < 0;
                    });
                } else {
                    data = dataRe.results;
                }
                $scope.setPagingData(data, $scope.page, $scope.pageSize);
            }
            else {
                $scope.setPagingData(dataRe.results, $scope.page, $scope.pageSize);
            }
        }

        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal.pageSize !== oldVal.pageSize || newVal.currentPage !== oldVal.currentPage) {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
            }
        }, true);
        $scope.$watch('filterOptions', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
            }
        }, true);

        $scope.gridOptions = {
            data: 'dayList',
            enablePaging: true,
            showFooter: true,
            totalServerItems: 'totalServerItems',
            pagingOptions: $scope.pagingOptions,
            filterOptions: $scope.filterOptions,
            columnDefs: 'columnDefs',
            enablePinning: true,
            keepLastSelected: true,
            multiSelect: false,
            showColumnMenu: true,
            showFilter: true,
            showGroupPanel: true,
            i18n: "en"
        };
    }
})();