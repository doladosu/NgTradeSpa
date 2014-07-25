angular.module('main')
    .controller('MainController', [
        '$q', '$timeout', 'datacontext', '$scope', '$location', '$window',
        function($q, $timeout, datacontext, $scope, $location, $window) {
            $scope.$root.title = 'NgTradeOnline';
            $scope.$on('$viewContentLoaded', function() {
                App.init();
                Index.initRevolutionSlider();
                OwlCarousel.initOwlCarousel();
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });

            $scope.selectedStock = '';
            $scope.getStock = function(searchText) {
                var ft = searchText.toLowerCase();
                return datacontext.getDailylist().
                    then(function(data) {
                        var stocksList = [];

                        data.results.filter(function(item) {
                            if (JSON.stringify(item.Stock).toLowerCase().indexOf(ft) != -1) {
                                stocksList.push(item.Stock);
                            }
                        });
                        return stocksList;
                    });
            };


            $scope.researchStock = function () {
                alert($scope.selectedStock);
            };
        }
    ]);