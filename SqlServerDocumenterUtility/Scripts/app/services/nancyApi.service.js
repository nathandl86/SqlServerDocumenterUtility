/*
 * Nancy Api Service
 * @namespace Services
 * @description Service for the app's Nancy configuration.
 */
(function (angular) {
    'use strict';/* jshint validthis: true */

    nancyApiService.$inject = ['commonService'];
    angular.module('sqlDocumenterApp').service('nancyApiService', nancyApiService);

    /*
    * @namespace NancyApi
    * @description Application wide nancy configuration
    * @memberOf Services
    */
    function nancyApiService(common) {
        var nancyEnabled = false,
            nancyApiPath = '';

        //methods
        this.useNancy = useNancy;
        this.setNancyApiPath = setNancyApiPath;
        this.getNancyApiPath = getNancyApiPath;

        return;

        /*
         * @name useNancy
         * @description Returns or sets if the application is currently configured to 
         *      use the Nancy api.
         * @param {boolean} use If defined, sets the applications configuration for nancy
         * @returns {boolean} If no arg passed in, returns the applications current setup for nancy.
         * @memberOf Services.NancyApi
         */
        function useNancy(use) {
            if (angular.isUndefined(use) || use === null) {
                return nancyEnabled;
            }
            else {
                nancyEnabled = use;
            }
        }

        /*
         * @name getNancyApiPath
         * @description Gets the api host and default path for the Nancy API
         * @returns {String}
         * @memberOf Services.NancyApi
         */
        function getNancyApiPath() {
            return nancyApiPath;
        }

        /*
         * @name setNancyApiPath
         * @description Sets the default nancy api host and path.
         * @param {String} Host path for Nancy API
         * @memberOf Services.NancyApi
         */
        function setNancyApiPath(path) {
            nancyApiPath = (path || '') + common.baseNancyUrl;
        }

    }

})(window.angular);