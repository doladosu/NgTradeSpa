angular.module('main')
    .controller('ContactController', ['$scope', function ($scope) {
        $(document).ready(function () {
            App.init();
            App.initSliders();
            ContactPage.initMap();
        });
    }]);