angular.module('main')
    .controller('MainController', ['$scope', function ($scope, $location, $window) {
        $scope.$root.title = 'NgTradeOnline';
        $scope.$on('$viewContentLoaded', function () {
            App.init();
            Index.initRevolutionSlider();
            OwlCarousel.initOwlCarousel();
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }]);