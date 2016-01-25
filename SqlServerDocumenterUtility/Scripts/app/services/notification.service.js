
/*
 * Notification Service
 * @namespace Factories
 * @description Factory for application notifications
 */
(function (angular, toastr) {
    'use strict';

    angular.module('sqlDocumenterApp').factory('notificationService', notificationService);

    /*
     * @namespace Notification
     * @description Application wide notification api
     * @memberOf Factories
     */
    function notificationService() {

        toastr.options = {
            "closeButton": true,
            "positionClass": "toast-bottom-center",
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        var service = {
            success: success,
            error: error,
            info: info
        };

        return service;

        /*
         * @name success
         * @description Handles success notifications
         * @param {String} text Message for notification
         * @param {String} header Header for the notification
         * @memberOf Notification
         */
        function success(text, header) {
            toastr.success(text, header || "Success");
        }

        /*
        * @name error
        * @description Handles error notifications
        * @param {String} text Message for notification
        * @param {String} header Header for the notification
        * @memberOf Notification
        */
        function error(text, header) {
            toastr.error(text, header || "Error");
        }

        /*
         * @name info
         * @description Handles info notifications
         * @param {String} text Message for notification
         * @param {String} header Header for the notification
         * @memberOf Notification
         */
        function info(text, header) {
            toastr.info(text, header || "Info");
        }
    }

})(window.angular, toastr);