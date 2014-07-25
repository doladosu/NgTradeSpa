angular.module('main')
    .controller('StaticController', [
        '$scope', function($scope, $location, $window) {
            $scope.$root.title = 'NgTradeOnline';
            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ]);