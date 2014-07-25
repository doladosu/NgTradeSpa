(function() {

    var navbarController = function($scope, $location, authService) {
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
            var path = '/login' + $location.$$path;
            $location.replace();
            $location.path(path);
        }

        $scope.$on('loginStatusChanged', function(loggedIn) {
            setLoginLogoutText(loggedIn);
        });

        $scope.$on('redirectToLogin', function() {
            redirectToLogin();
        });

        function setLoginLogoutText() {
            $scope.loginLogoutText = (authService.user.isAuthenticated) ? 'Logout' : 'Login';
        }

        setLoginLogoutText();

    };

    navbarController.$inject = ['$scope', '$location', 'authService'];

    angular.module('main').controller('NavbarController', navbarController);

}());
