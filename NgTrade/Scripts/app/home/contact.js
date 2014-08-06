angular.module('main')
    .controller('ContactController', [
        '$scope', '$location', '$window', 'httpService', 'logger', function ($scope, $location, $window, httpService, logger) {
            $scope.$root.title = 'NgTradeOnline';
            $scope.$on('$viewContentLoaded', function () {
                App.init();
                ContactPage.initMap();
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
            $scope.name = null;
            $scope.email = null;
            $scope.message = null;
            $scope.isEmailValid = true;

            $scope.contact = function () {
                httpService.contact($scope.name, $scope.email, $scope.message).then(function (status) {
                    if (!status) {
                        logger.error("Please check your username and password", "Login failed");
                        return;
                    }
                });
            };
        }
    ]);