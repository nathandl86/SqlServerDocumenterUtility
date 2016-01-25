
/*
 * Extended Property Service
 * @namespace Factories
 * @description Factory for the http api interactions using Web API and/or Nancy
 */
(function (angular) {
    'use strict';

    extendedPropertyService.$inject = ['$http', '$q', 'commonService', 'nancyApiService'];
    angular.module('sqlDocumenterApp').factory('extendedPropertyService', extendedPropertyService);

    /*
     * @namespace ExtendedPropertyService
     * @description Api for http interaction
     * @memberOf Factories
     */
    function extendedPropertyService(ngHttp, ngQ, common, nancyApi) {
        var apiPrefix = common.appRoot + common.baseAppUrl + common.baseWebApiUrl + 'property/';

        var service = {
            deleteProperty: deleteProperty,
            addProperty: addProperty,
            updateProperty: updateProperty,
            getProperties: getTableProperties
        };

        return service;

        /*
         * @name deleteProperty
         * @description Removes extended property from table or column
         * @param {Object} propertyModel
         * @param {Object} connection
         * @returns {Promise}
         * @memberOf ExtendedPropertyService
         */
        function deleteProperty(propertyModel, connection) {
            ngHttp.defaults.headers.common.connectionString = connection.connectionString;
            return ngHttp({
                method: 'POST',
                url: buildUrl('delete'),
                data: propertyModel
            }).then(function () {
                common.notifier.success("The property has been deleted!", "Deletion Successful");
            }, function (err) {
                common.logger.log(err);
                var msg = getErrorMsg(err, "Failed to delete property");
                common.notifier.error(msg, getErrorHeader(err));
            });
        }

        /*
        * @name addProperty
        * @description Adds extended property from table or column
        * @param {Object} propertyModel
        * @param {Object} connection
        * @returns {Promise}
        * @memberOf ExtendedPropertyService
        */
        function addProperty(propertyModel, connection) {
            ngHttp.defaults.headers.common.connectionString = connection.connectionString;
            return ngHttp({
                method: 'POST',
                url: buildUrl('add'),
                data: propertyModel
            }).then(function () {
                common.notifier.success("The property has been added!", "Save Successful");
            }, function (err) {
                common.logger.log(err);
                var msg = getErrorMsg(err, "Failed to add the property");
                common.notifier.error(msg, getErrorHeader(err));
            });
        }

        /*
         * @name updateProperty
         * @description Updates extended property from table or column
         * @param {Object} propertyModel
         * @param {Object} connection
         * @returns {Promise}
         * @memberOf ExtendedPropertyService
         */
        function updateProperty(propertyModel, connection) {
            ngHttp.defaults.headers.common.connectionString = connection.connectionString;
            return ngHttp({
                method: 'POST',
                url: buildUrl('update'),
                data: propertyModel
            }).then(function () {
                common.notifier.success("The property has been updated!", "Save Successful");
            }, function (err) {
                common.logger.log(err);
                var msg = getErrorMsg(err, "Failed to update the property");
                common.notifier.error(msg, getErrorHeader(err));
            });
        }

        /*
        * @name getTableProperties
        * @description Gets extended property for a table and its columns
        * @param {integer} tableId
        * @param {Object} connection
        * @returns {Promise}
        * @memberOf ExtendedPropertyService
        */
        function getTableProperties(tableId, connection) {
            ngHttp.defaults.headers.common.connectionString = connection.connectionString;
            return ngHttp({
                method: 'GET',
                url: buildUrl('get/' + tableId)
            }).then(function (resp) {
                return resp.data;
            }, function (err) {
                common.logger.log(err);
                var msg = getErrorMsg(err, "Failed to get properties for the selected table");
                common.notifier.error(msg, getErrorHeader(err));
                return [];
            });
        }

        function getErrorMsg(err, defaultMsg) {
            return err.status === 400 ? (err.data.Message || defaultMsg) : defaultMsg;
        }

        function getErrorHeader(err) {
            return err.status + ' | ' + err.statusText;
        }

        function buildUrl(relative) {
            if (nancyApi.useNancy()) {
                return nancyApi.getNancyApiPath() + "property/" + relative;
            }
            else {
                return apiPrefix + relative;
            }
        }

    }

})(window.angular);