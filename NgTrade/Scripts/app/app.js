var appRoot = angular.module('main', ['ngRoute', 'ngGrid', 'ngResource', 'breeze.angular', 'ngAnimate', 'ngSanitize', 'mgcrea.ngStrap']);//, 'angularStart.filters']); 

appRoot
    .config([
        '$routeProvider', function($routeProvider) {
            $routeProvider
                .when('/home', { templateUrl: '/home/main', controller: 'MainController' })
                .when('/nsecontactus', { templateUrl: '/home/contact', controller: 'ContactController' })
                .when('/about', { templateUrl: '/home/about', controller: 'StaticController' })
                .when('/nsecontactus', { templateUrl: '/home/nsecontactus', controller: 'ContactController' })
                .when('/nseeducation', { templateUrl: '/home/nseeducation', controller: 'StaticController', secure: true })
                .when('/nsenews', { templateUrl: '/home/nsenews', controller: 'StaticController' })
                .when('/nsefaq', { templateUrl: '/home/nsefaq', controller: 'StaticController' })
                .when('/nseprivacy', { templateUrl: '/home/nseprivacy', controller: 'StaticController' })
                .when('/nseterms', { templateUrl: '/home/nseterms', controller: 'StaticController' })
                .when('/research', { templateUrl: '/home/nseresearch', controller: 'StaticController' })
                .when('/daygainers', { templateUrl: '/home/daygainers', controller: 'StaticController' })
                .when('/daylosers', { templateUrl: '/home/daylosers', controller: 'StaticController' })
                .when('/dailypricelist', { templateUrl: '/home/nsedailypricelist', controller: 'DailyListController' })
                .when('/login', { templateUrl: '/account/login', controller: 'LoginController' })
                .when('/register', { templateUrl: '/account/register', controller: 'RegisterController' })
                .when('/privacy', { templateUrl: '/home/nseprivacy', controller: 'StaticController' })
                .when('/terms', { templateUrl: '/home/nseterms', controller: 'StaticController' })
                .when('/account', { templateUrl: '/user/account', controller: 'AccountController', secure: true })
                .otherwise({ redirectTo: '/home' });
        }
    ])
    .controller('RootController', [
        '$scope', '$route', '$routeParams', '$location', function($scope, $route, $routeParams, $location) {
            $scope.$on('$routeChangeSuccess', function(e, current, previous) {
                $scope.activeViewPath = $location.path();
            });
        }
    ])
    .run([
        'breeze', '$rootScope', '$location', 'authService',
        function(breeze, $rootScope, $location, authService) {
            //Client-side security. Server-side framework MUST add it's 
            //own security as well since client-based security is easily hacked
            $rootScope.$on("$routeChangeStart", function(event, next, current) {
                if (next && next.$$route && next.$$route.secure) {
                    if (!authService.user.isAuthenticated) {
                        authService.redirectToLogin();
                    }
                }
            });

        }
    ]);
