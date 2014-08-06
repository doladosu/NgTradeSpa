angular.module('main')
    .controller('NewsController', [
        '$scope', '$location', '$window', function ($scope, $location, $window) {
            $scope.$root.title = 'NgTradeOnline - Nigerian stock exchange news';
            $scope.$on('$viewContentLoaded', function () {
                App.init();
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ]);