(function() {

    var authFactory = function($http, $rootScope) {
        var serviceBase = '/api/auth/',
            factory = {
                loginPath: '/login',
                user: {
                    isAuthenticated: false,
                    roles: null
                }
            };

        factory.login = function(username, password) {
            return $http.post(serviceBase + 'login', { userName: username, Password: password }).then(
                function(results) {
                    var loggedIn = results.data == "true";;
                    changeAuth(loggedIn);
                    return loggedIn;
                });
        };

        factory.logout = function() {
            return $http.post(serviceBase + 'logout').then(
                function(results) {
                    var loggedIn = results.data == "true";
                    changeAuth(loggedIn);
                    return loggedIn;
                });
        };

        factory.redirectToLogin = function() {
            $rootScope.$broadcast('redirectToLogin', null);
        };

        function changeAuth(loggedIn) {
            factory.user.isAuthenticated = loggedIn;
            $rootScope.$broadcast('loginStatusChanged', loggedIn);
        }

        return factory;
    };

    authFactory.$inject = ['$http', '$rootScope'];

    angular.module('main').factory('authService', authFactory);

}());