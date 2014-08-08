(function () {

    var registerController = function ($scope, $location, $window, $routeParams, authService, logger) {

        $scope.$root.title = 'NgTradeOnline - Register';
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });

        var path = '/account/index';
        $scope.username = null;
        $scope.firstname = null;
        $scope.lastname = null;
        $scope.password = null;
        $scope.confirmpassword = null;
        $scope.address1 = null;
        $scope.address2 = null;
        $scope.city = null;
        $scope.state = null;
        $scope.country = null;
        $scope.phone = null;
        $scope.errorMessage = null;
        $scope.isEmailValid = true;

        $scope.register = function () {
            authService.register($scope.username, $scope.firstname, $scope.lastname, $scope.password,
                $scope.confirmpassword, $scope.address1, $scope.address2, $scope.city, $scope.state, $scope.country, $scope.phone).then(function (status) {
                if (!status) {
                    logger.error("Please check your username and password", "Login failed");
                    $scope.errorMessage = 'Unable to login';
                    return;
                }

                if (status && $routeParams && $routeParams.redirect) {
                    path = $routeParams.redirect;
                }
                $window.location.href = path;
                // $location.path(path);
            });
        };
    };

    registerController.$inject = ['$scope', '$location', '$window', '$routeParams', 'authService', 'logger'];

    angular.module('main')
        .controller('RegisterController', registerController);

}());