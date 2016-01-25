/*
 * Custom Dropdown
 * @namespace Directives
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp').directive('customDropdown', customDropdown);

    /*
    * @namespace CustomDropdown
    * @description Directive encapsulating a common dropdown
    * @memberOf Directives
    */
    function customDropdown() {

        var directive = {
            restrict: 'E',
            replace: true,
            require: '?ngModel',
            scope: {
                items: '=',
                text: '=',
                textPath: '@?',
                textCaption: '@?',
                selected: '=',
                disabled: '=',
                cssClass: '@?'
            },
            templateUrl: '/Scripts/app/views/dropdown.directive.html',
            controller: 'dropdownController',
            controllerAs: 'drop',
            bindToController: true,
            link: angular.noop
        };

        return directive;
    }


})(window.angular);