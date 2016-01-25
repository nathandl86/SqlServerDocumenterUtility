/*
 * Property Model Builder Service
 * @namespace Factories
 * @description Factory for creation of a property model
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp').factory('propertyModelBuilderService', propertyModelBuilderService);

    /*
     * @namespace PropertyModelBuilder
     * @description Creates a property model
     * @memberOf Factories
     */
    function propertyModelBuilderService() {
        
        var service = {
            create: create
        };

        return service;

        /*
         * @name create
         * @description Creates model
         * @param {Object} details Details for new-ing of PropertyModel object.
         * @returns {PropertyModel} 
         * @memberOf PropertyModelBuilder
         */
        function create(details) {
            if (angular.isUndefined(details) || details === null) {
                return null;
            }
            else {
                return new PropertyModel(details);
            }
        }
    }

})(window.angular);