(function () {

    var httpFactory = function ($http, $rootScope) {
        var serviceBase = '/api/user/',
            factory = {
                contactPath: '/contact',
                user: {
                    isAuthenticated: false,
                    roles: null
                }
            };

        factory.contact = function (name, email, message) {
            return $http.post(serviceBase + 'contact', { name: name, email: email, message: message }).then(
                function (results) {

                });
        };

        return factory;
    };

    httpFactory.$inject = ['$http', '$rootScope'];

    angular.module('main').factory('httpService', httpFactory);

}());