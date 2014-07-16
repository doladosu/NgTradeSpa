//angular.module('main')
//    .controller('LoginController', ['$scope', function ($scope) {
//        $(document).ready(function () {
//            App.init();
//        });
//    }]);

(function () {

    var loginController = function ($scope, $location, $routeParams, authService) {
        var path = '/';
        $scope.email = null;
        $scope.password = null;
        $scope.errorMessage = null;
        $scope.isEmailValid = true;

        $scope.login = function () {
            authService.login($scope.email, $scope.password).then(function (status) {
                //$routeParams.redirect will have the route
                //they were trying to go to initially
                if (!status) {
                    $scope.errorMessage = 'Unable to login';
                    return;
                }

                if (status && $routeParams && $routeParams.redirect) {
                    path = path + $routeParams.redirect;
                }

                $location.path(path);
            });
        };
    };

    loginController.$inject = ['$scope', '$location', '$routeParams', 'authService'];

    angular.module('main')
        .controller('LoginController', loginController);

}());