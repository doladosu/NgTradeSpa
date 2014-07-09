var appRoot = angular.module('main', ['ngRoute', 'ngGrid', 'ngResource', 'breeze.angular']);//, 'angularStart.filters']); 

appRoot
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/home', { templateUrl: '/home/main', controller: 'MainController' })
            .when('/nsecontactus', { templateUrl: '/home/contact', controller: 'ContactController' })
            .when('/about', { templateUrl: '/home/about', controller: 'AboutController' })
            .when('/demo', { templateUrl: '/home/demo', controller: 'DemoController' })
            .when('/angular', { templateUrl: '/home/angular' })
            .when('/nsecontactus', { templateUrl: '/home/nsecontactus', controller: 'ContactController' })
            .when('/nseeducation', { templateUrl: '/home/nseeducation', controller: 'StaticController' })
            .when('/nsenews', { templateUrl: '/home/nsenews', controller: 'StaticController' })
            .when('/nsefaq', { templateUrl: '/home/nsefaq', controller: 'StaticController' })
            .when('/nseprivacy', { templateUrl: '/home/nseprivacy', controller: 'StaticController' })
            .when('/nseterms', { templateUrl: '/home/nseterms', controller: 'StaticController' })
            .when('/research', { templateUrl: '/home/research', controller: 'StaticController' })
            .when('/daygainers', { templateUrl: '/home/daygainers', controller: 'StaticController' })
            .when('/daylosers', { templateUrl: '/home/daylosers', controller: 'StaticController' })
            .when('/dailypricelist', { templateUrl: '/home/dailypricelist', controller: 'StaticController' })
            .when('/login', { templateUrl: '/account/login', controller: 'LoginController' })
            .when('/register', { templateUrl: '/account/register', controller: 'RegisterController' })
            .when('/privacy', { templateUrl: '/home/nseprivacy', controller: 'StaticController' })
            .when('/terms', { templateUrl: '/home/nseterms', controller: 'StaticController' })
            .otherwise({ redirectTo: '/home' });
    }])
    .controller('RootController', ['$scope', '$route', '$routeParams', '$location', function ($scope, $route, $routeParams, $location) {
        $scope.$on('$routeChangeSuccess', function (e, current, previous) {
            $scope.activeViewPath = $location.path();
        });
    }]);
