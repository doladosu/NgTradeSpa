(function() {

    var authFactory = function($http, $rootScope) {
        var serviceBase = '/api/auth/',
            factory = {
                loginPath: '/login',
                registerPath: '/register',
                user: {
                    isAuthenticated: false,
                    roles: null
                }
            };

        factory.login = function(username, password) {
            return $http.post(serviceBase + 'login', { userName: username, Password: password }).then(
                function(results) {
                    var loggedIn = results.data.userId > 0;;
                    changeAuth(loggedIn);
                    return loggedIn;
                });
        };

        factory.logout = function() {
            return $http.post(serviceBase + 'logout').then(
                function(results) {
                    var loggedIn = results.data.userId > 0;
                    changeAuth(loggedIn);
                    return loggedIn;
                });
        };

        factory.register = function(username, firstname, lastname, password, confirmpassword, address1, address2, city, state, country, phone) {
            return $http.post(serviceBase + 'register', {
                userName: username,
                firstname: firstname,
                lastname: lastname,
                Password: password,
                confirmpassword: confirmpassword,
                address1: address1,
                address2: address2,
                city: city,
                state: state,
                country: country,
                phone: phone
            }).then(
                function(results) {
                    var loggedIn = results.data.userId > 0;;
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