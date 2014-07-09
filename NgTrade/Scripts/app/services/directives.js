app.directive('disable', function($interpolate) {
        return function(scope, elem, attrs) {
            var exp = $interpolate(elem.attr('data-disable'));

            function updateDisabled() {
                var val = scope.$eval(exp);
                if (val == "true") {
                    elem[0].disabled = 'disabled';
                } else {
                    elem[0].disabled = '';
                }
            }

            scope.$watch(exp, function(value) {
                updateDisabled();
            });
        }
    })
    .directive('flash', function($) {
        return function(scope, elem, attrs) {
            var flag = elem.attr('data-flash');
            var $elem = $(elem);

            function flashRow() {
                var value = scope.stock.LastChange;
                var changeStatus = scope.$eval(flag);
                if (changeStatus) {
                    var bg = value === 0
                        ? '255,216,0' // yellow
                        : value > 0
                        ? '154,240,117' // green
                        : '255,148,148'; // red

                    $elem.flash(bg, 1000);
                }
            }

            scope.$watch(flag, function(value) {
                flashRow();
            });
        }
    })
    .directive('scrollTicker', function($) {
        return function(scope, elem, attrs) {
            var $scrollTickerUi = $(elem);
            var flag = elem.attr('data-scroll-ticker');
            scroll();

            function scroll() {
                if (scope.$eval(flag)) {
                    var w = $scrollTickerUi.width();
                    $scrollTickerUi.css({ marginLeft: w });
                    $scrollTickerUi.animate({ marginLeft: -w }, 15000, 'linear', scroll);
                } else
                    $scrollTickerUi.stop();
            }

            scope.$watch(flag, function(value) {
                scroll();
            });
        }
    });
