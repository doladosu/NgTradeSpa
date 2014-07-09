angular.module('main')
    .controller('MainController', ['$scope', function ($scope) {
        $(document).ready(function () {
            App.init();
            Index.initRevolutionSlider();
            OwlCarousel.initOwlCarousel();
        });
    }]);