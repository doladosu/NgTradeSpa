(function () {

    var userFactory = function ($http, $rootScope) {
        var serviceBase = '/api/user/',
            factory = {
                loginPath: '/getprofile',
                user: {
                    isAuthenticated: false,
                    roles: null
                }
            };

        factory.getprofile = function () {
            return $http.get(serviceBase + 'getprofile', { }).then(
                function (results) {
                    return results.data;
                });
        };

        return factory;
    };

    userFactory.$inject = ['$http', '$rootScope'];

    angular.module('smartApp').factory('userService', userFactory);

}());