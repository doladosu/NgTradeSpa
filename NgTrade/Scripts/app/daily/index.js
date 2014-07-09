/* TodoController - the controller for the "todo view" 
 * relies on Angular injector to provide:
 *     $q - promises manager
 *     $timeout - Angular equivalent of 'setTimeout'
 *     dataservice - the application data access service
 *     logger - the application's logging facility
 */
(function () {

    angular.module('main').controller('DailyListController',
    ['$q', '$timeout', 'datacontext', '$scope', controller]);

    function controller($q, $timeout, datacontext, $scope) {
        // The controller's API to which the view binds
        var vm = this;
        $scope.usersList = [];

        vm.getDailylist = getDailylist;

        getDailylist();

        // Listen for property change of ANY entity so we can (optionally) save
        datacontext.addPropertyChangeHandler(propertyChanged);

        /* Implementation */

        function getDailylist() {
            //editEnd();
            // wait for Ng binding to set 'includeArchived' flag, then proceed
            $timeout(getDailylistImpl, 0);

            function getDailylistImpl() {
                datacontext.getDailylist()
                    .then(querySucceeded);
            }

            function querySucceeded(data) {
                vm.items = data.results;
                $scope.usersList = data.results;
                alert(data.results);
            }
        };

        $scope.userGrid = {
            data: 'usersList',
            multiSelect: false,
            enableColumnResize: false,
            columnDefs: [
                { field: 'Date', displayName: 'Date', width: '25%' },
                { field: 'Stock', displayName: 'Stock', width: '25%' },
                { field: 'Open', displayName: 'Open', width: '25%' },
                { field: 'High', displayName: 'High', width: '25%' }
            ]
        };

        function propertyChanged(changeArgs) {
            // propertyChanged triggers save attempt UNLESS the property is the 'Id'
            // because THEN the change is actually the post-save Id-fixup 
            // rather than user data entry so there is actually nothing to save.
            if (changeArgs.args.propertyName !== 'Id') {
                save();
            }
        }
    }

})();