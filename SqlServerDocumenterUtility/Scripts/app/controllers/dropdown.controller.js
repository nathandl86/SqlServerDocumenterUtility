/*
 * Custom Dropdown Controller
 * @namespace Controllers
 */
(function (angular) {
    'use strict';

    dropdownController.$inject = [];
    angular.module('sqlDocumenterApp').controller('dropdownController', dropdownController);

    /*
     * @namespace CustomDropdownController
     * @description Controller for the custom drodown directive
     * @memberOf Controllers
     */
    function dropdownController() {
        /* jshint validthis: true */
        var vm = this,
            defaultText = vm.textCaption || '-- Select an Item --';

        //properties
        vm.disabled = vm.disabled || false;
        vm.open = false;
        vm.cssClass = vm.cssClass || "";
        vm.text = vm.text || defaultText;

        //methods
        vm.isOpen = isOpen;
        vm.isDisabled = isDisabled;
        vm.select = select;
        vm.getText = getText;        
        vm.getSelectedText = getSelectedText;

        /*
         * @name getText
         * @description Determines the text string from an item
         * @param {Object} item
         * @returns {String}
         * @memberOf CustomDropdownController
         */
        function getText(item) {
            if (angular.isDefined(item) && item) {
                return item[vm.textPath] || item.toString();
            }
        }

        /*
         * @name getSelectedText
         * @description Gets the text of the selected item
         * @returns {String}
         * @memberOf CustomDropdownController
        */
        function getSelectedText() {
            var selectedItemText = getText(vm.selected);
            return selectedItemText || vm.text;
        }

        /*
         * @name select
         * @description selection handler from the dropown when an item is selected
         * @param {Object} $e Event args
         * @param {Object} data Selected item data
         * @memberOf CustomDropdownController
         */
        function select($e, data) {
            vm.selected = data;
            vm.text = vm.selected[vm.textPath] || data.toString();
        }

        /*
         * @name isDisabled
         * @description Determines if the dropdown should be disabled
         * @returns {boolean}
         * @memberOf CustomDropdownController
         */
        function isDisabled() {
            if (vm.disabled) return true;
            if (vm.items === null) return true;
            if (vm.items.length === 0) return true;
            return false;
        }

        /*
         * @name isOpen
         * @description Determines if the dropdown is open
         * @returns {boolean}
         * @memberOf CustomDropdownController
         */
        function isOpen() {
            return vm.open;
        }
    }

})(window.angular);