(function() {

    var navbarController = function($scope, $location, authService, $routeParams) {
        var appTitle = 'NgTradeOnline';
        $scope.isCollapsed = false;
        $scope.appTitle = appTitle;

        $scope.highlight = function(path) {
            return $location.path().substr(0, path.length) == path;
        };

        $scope.loginOrOut = function() {
            setLoginLogoutText();
            var isAuthenticated = authService.user.isAuthenticated;
            if (isAuthenticated) { //logout 
                authService.logout().then(function() {
                    $location.path('/');
                    return;
                });
            }
            redirectToLogin();
        };

        function redirectToLogin() {
            $routeParams.redirect = $location.$$path;
            var path = '/login';
            $location.replace();
            $location.path(path);
        }

        $scope.$on('loginStatusChanged', function(loggedIn) {
            setLoginLogoutText(loggedIn);
        });

        $scope.$on('registerStatusChanged', function (loggedIn) {
            setRegisterLogoutText(loggedIn);
        });

        $scope.$on('redirectToLogin', function() {
            redirectToLogin();
        });

        function setLoginLogoutText() {
            $scope.loginLogoutText = (authService.user.isAuthenticated) ? 'Logout' : 'Login';
        }

        function setRegisterLogoutText() {
            $scope.registerLogoutText = (authService.user.isAuthenticated) ? '' : 'Register';
        }

        setLoginLogoutText();
        setRegisterLogoutText();
    };

    navbarController.$inject = ['$scope', '$location', 'authService', '$routeParams'];

    angular.module('main').controller('NavbarController', navbarController);

}());
