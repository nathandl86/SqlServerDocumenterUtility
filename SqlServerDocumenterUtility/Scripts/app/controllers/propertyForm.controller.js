/*
 * Property Form Controller
 * @namespace Controllers
 */
(function (angular) {
    'use strict';/* jshint validthis: true */

    propertyFormController.$inject = ['$scope', '$timeout', 'propertySuggestionsService'];
    angular.module('sqlDocumenterApp').controller('propertyFormController', propertyFormController);

    /*
     * @namespace PropertyFormController
     * @description Controller for the property form directive
     * @memberOf Controllers
     */
    function propertyFormController(ngScope, ngTimeout, suggestionService) {
        var vm = this;

        //properties
        vm.name = '';
        vm.description = null;
        vm.suggestions = [];

        //methods
        vm.submit = submit;
        vm.clear = clear;
        vm.isFormDisabeld = isFormDisabeld;

        //watch(es)
        ngScope.$watch(function () {
            return vm.property;
        }, propertyChanged);

        init();
        return;

        //functions
        function init() {
            vm.suggestions = suggestionService.get(vm.type);
        }

        /*
         * @name submit
         * @description submits changes to existing or creation of a new extended property
         * @memberOf PropertyFormController
         */
        function submit() {
            vm.saved = {
                objectId: vm.objectId,
                propertyName: vm.name,
                propertyValue: vm.description
            };
        }

        /*
         * @name clear
         * @description clears the form
         * @memberOf PropertyFormController
         */
        function clear() {
            vm.name = '';
            vm.description = null;
            vm.property = null;
        }

        /*
         * @name isFormDisabled
         * @description Determining if the property form should be enabled or not
         * @returns {boolean}
         * @memberOf PropertyFormController
         */
        function isFormDisabeld() {
            return angular.isUndefined(vm.objectId) || vm.objectId === null || vm.objectId === '';
        }

        function propertyChanged(val) {
            if (angular.isDefined(val) && val) {
                val = JSON.parse(val);
                vm.name = val.name;
                vm.description = val.description;
                vm.objectId = val.objectId;
            }
        }        

    }

})(window.angular);