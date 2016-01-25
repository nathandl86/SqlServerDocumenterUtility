/*
 * Common Service
 * @namespace Services
 * @description Service housing common functionality needed with the angular code
 */
(function (angular) {
    'use strict';
    /* jshint validthis: true */
    
    commonService.$inject = ['notificationService', 'loggerService', 'baseUrl', 'nancyUrl'];
    angular.module('sqlDocumenterApp').service('commonService', commonService);

    /*
     * @namespace Common
     * @description Application wide common service
     * @memberOf Services
     */
    function commonService(notify, log, baseUrl, nancyUrl) {        
        this.notifier = notify;
        this.logger = log;
        this.baseAppUrl = baseUrl;
        this.baseWebApiUrl = 'api/';
        this.baseNancyUrl = nancyUrl;
        this.appRoot = '';
    }

})(window.angular);