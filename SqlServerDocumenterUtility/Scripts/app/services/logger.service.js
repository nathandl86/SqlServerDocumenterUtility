/*
 * Logger Service
 * @namespace Factories
 * @description Factory for application logging
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp').factory('loggerService', loggerService);

    /*
     * @namespace Logger
     * @description Application wide logging api
     * @memberOf Factories
     */
    function loggerService(){
        
        var service = {
            log: log
        };

        return service;

        /*
         * @name log
         * @description Handles log requests
         * @param {object} data Object of what the issuer wishes to log
         * @memberOf Logger
         */
        function log(data){
            console.log(data);
        }
    }

})(window.angular);