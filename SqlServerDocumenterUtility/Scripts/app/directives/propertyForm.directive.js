/*
 * Property Form
 * @namespace Directives
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp').directive('propertyForm', propertyFormDirective);

    /*
     * @namespace PropertyForm
     * @description Directive encapsulating the functionality for creating properties
     * @memberOf Directives
     */
    function propertyFormDirective() {
        var directive = {
            restrict: 'EA',
            replace: false,
            scope: {
                objectId: '@',
                property: "@",
                type: '@',
                saved: '=',
            },
            templateUrl: '/Scripts/app/views/propertyForm.directive.html',
            controller: 'propertyFormController',
            controllerAs: 'propertyForm',
            bindToController: true
        };

        return directive;
    }

})(window.angular);