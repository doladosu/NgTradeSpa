require.config({
    paths: {
        'jquery': '../jquery-2.1.1.min',
        'jquery-migrate-1.2.1.min': '../../assets/plugins/jquery-migrate-1.2.1.min',
        'bootstrap.min': '../bootstrap.min',
        'jquery.color-2.1.2.min': '../jquery.color-2.1.2.min',
        'jQueryColor-FlashEffect': '../jQueryColor-FlashEffect',
        'jquery.signalR-2.1.0.min': '../jquery.signalR-2.1.0.min',
        'angular': '../angular',
        'angularAMD': '//cdn.jsdelivr.net/angular.amd/0.1.1/angularAMD.min',
        'angular-route': '../angular-route',
        'angular-animate': '../angular-animate',
        'angular-ng-grid': '../angular-ng-grid',
        'q': '../q.min',
        'breeze': '../breeze.min',
        'breeze.savequeuing': '../breeze.savequeuing',
        'toastr': '../toastr',
        'phelper': '../phelper',
        'back-to-top': '../../assets/plugins/back-to-top',
        'owl.carousel': '../../assets/plugins/owl-carousel/owl-carousel/owl.carousel',
        'jquery.themepunch.revolution.min': '../../assets/plugins/revolution_slider/rs-plugin/js/jquery.themepunch.revolution.min',
        'assetsapp': '../../assets/js/app',
        'index': '../../assets/js/pages/index',
        'gmap': '../../assets/plugins/gmap/gmap',
        'page_contacts': '../../assets/js/pages/page_contacts'
    },
    baseUrl: 'scripts/app',
    shim: {
        'angularAMD': ['angular'],
        'angular-route': ['angular'],
        'angular-animate': ['angular'],
        'angular-ng-grid': ['angular'],
        'angular': {
            deps: ["q", "jquery", "breeze", "toastr"],
            exports: "angular"
        },
        'bootstrap': {
            deps: ['jquery']
        }
    },
    priority: [
        'angular'
    ],

    deps: ['app']
});