/*
 * Documenter
 * @namespace Directives 
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp').directive('documenter', documenter);

    /*
    * @name documenter
    * @description Directive for the components for associated extended properties to sql server tables.
    * @memberOf Directives
    */
    function documenter() {
        var directive = {
            restrict: 'E',
            replace: true,
            scope: {
                table: '=',
                connection: '='
            },
            templateUrl: '/Scripts/app/views/documenter.directive.html',
            controller: 'documenterController',
            controllerAs: 'documenter',
            bindToController: true,
            link: angular.noop
        };

        return directive;
    }

})(window.angular);