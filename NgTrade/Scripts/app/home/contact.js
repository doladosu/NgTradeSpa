angular.module('main')
    .controller('ContactController', [
        '$scope', '$location', '$window', function ($scope, $location, $window) {
            $scope.$root.title = 'NgTradeOnline';
            $scope.$on('$viewContentLoaded', function() {
                App.init();
                App.initSliders();
                ContactPage.initMap();
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ]);