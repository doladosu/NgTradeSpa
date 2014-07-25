//angular.module('main')
//    .controller('LoginController', ['$scope', function ($scope) {
//        $(document).ready(function () {
//            App.init();
//        });
//    }]);

(function() {

    var loginController = function ($scope, $location, $window, $routeParams, authService) {

        $scope.$root.title = 'NgTradeOnline - Login';
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });

        var path = '/account';
        $scope.username = null;
        $scope.password = null;
        $scope.errorMessage = null;
        $scope.isEmailValid = true;

        $scope.login = function() {
            authService.login($scope.username, $scope.password).then(function(status) {
                if (!status) {
                    $scope.errorMessage = 'Unable to login';
                    return;
                }

                if (status && $routeParams && $routeParams.redirect) {
                    path = $routeParams.redirect;
                }

                $location.path(path);
            });
        };
    };

    loginController.$inject = ['$scope', '$location', '$window', '$routeParams', 'authService'];

    angular.module('main')
        .controller('LoginController', loginController);

}());