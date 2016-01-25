/*
 * Documenter Service
 * @namespace Factories
 * @description Factory for http api interactions using Web API and/or Nancy
 */
(function (angular, _) {
    'use strict';

    documenterService.$inject = ['$http', '$q', '$timeout', 'commonService', 'nancyApiService'];
    angular.module('sqlDocumenterApp').factory('documenterService', documenterService);

    /*
     * @namespace DocumenterService
     * @description Api for http interactions
     * @memberOf Factories
     */
    function documenterService(ngHttp, ngQ, ngTimeout, common, nancyApi) {
        var apiPrefix = common.appRoot + common.baseAppUrl + common.baseWebApiUrl + 'documenter/';

        var service = {
            getDatabases: getDatabases,
            getTables: getTables,
            getColumns: getColumns
        };

        return service;

        /*
        * @name getDatabases
        * @description Gets a list of databases accessible for documentation within the app. Will 
        *      cross reference Web API results with Nancy results. Matches result in the Web API
        *      connection also being Nancy enabled.
        * @returns {Promise}
        * @memberOf DocumenterService
        */
        function getDatabases() {
            var deferred = ngQ.defer(),
                webApiDbs = null,
                nancyDbs = null;

            getWebApiDatabases().
                then(function (data) {
                    webApiDbs = data;
                    if (ready()) {
                        deferred.resolve(buildReturn());
                    }
                });

            getNancyDatabases().
                then(function (data) {
                    nancyDbs = data;
                    if (ready()) {
                        deferred.resolve(buildReturn());
                    }
                });

            return deferred.promise;

            function ready() {
                if (webApiDbs !== null && nancyDbs !== null) {
                    return true;
                }
                return false;
            }

            function buildReturn() {
                var returnVal = [];
                if (webApiDbs.length === 0 && nancyDbs.length === 0) {
                    return returnVal;
                }

                for (var i = 0, len = webApiDbs.length; i < len; i++) {
                    var connection = webApiDbs[i];
                    connection.allowNancy = (typeof isNancyEnabled(connection) !== "undefined");
                    returnVal.push(connection);
                }
                return returnVal;

                function isNancyEnabled(connection) {

                    return _.find(nancyDbs, function (nDb) {
                        return nDb.name === connection.name;
                    });
                }
            }
        }

        /*
         * @name getTables
         * @description Gets a list of tables for the database connection arg
         * @param {Object} connection Connection object with details on the data connection to use.
         * @returns {Promise}
         * @memberOf DocumenterService
         */
        function getTables(connection) {
            ngHttp.defaults.headers.common.connectionString = connection.connectionString;
            return ngHttp({
                method: 'GET',
                url: buildUrl('tables')
            }).then(function (resp) {
                return resp.data;
            }, function (err) {
                common.logger.log(err); //here is a place where appropriate handling should take place
                common.notifier.error("Failed to find tables", getErrorHeader(err));
                return [];
            });
        }

        /*
        * @name getColumns
        * @description Gets a list of columns for selected table
        * @param {integer} tableId Table identifier
        * @param {Object} connection Connection object with details on the data connection to use.
        * @returns {Promise}
        * @memberOf DocumenterService
        */
        function getColumns(tableId, connection) {
            ngHttp.defaults.headers.common.connectionString = connection.connectionString;
            return ngHttp({
                method: 'GET',
                url: buildUrl('columns/' + tableId)
            }).then(function (resp) {
                return resp.data;
            }, function (err) {
                common.logger.log(err); //appropriate error handling here.
                common.notifier.error("Failed to find columns for selected table", getErrorHeader(err));
                return [];
            });
        }

        function buildUrl(relative) {
            if (nancyApi.useNancy()) {
                return nancyApi.getNancyApiPath() + relative;
            }
            else {
                return apiPrefix + relative;
            }
        }

        function getErrorHeader(err) {
            return err.status + ' | ' + err.statusText;
        }

        function getWebApiDatabases() {
            return ngHttp({
                method: 'GET',
                url: apiPrefix + 'databases'
            }).then(function (resp) {
                return resp.data;
            }, function (err) {
                common.logger.log(err); //here is a place where appropriate handling should take place
                common.notifier.error("Failed to find databases setup for Web API", getErrorHeader(err));
                return [];
            });
        }

        function getNancyDatabases() {
            var host = nancyApi.getNancyApiPath();

            if (angular.isUndefined(host) || host === null || host.trim() === '') {
                var deferred = ngQ.defer();
                ngTimeout(function () {
                    deferred.resolve([]);
                }, 1);
                return deferred.promise;
            }

            return ngHttp({
                method: 'GET',
                url: host + 'databases'
            }).then(function (resp) {
                return resp.data;
            }, function (err) {
                common.logger.log(err); //here is a place where appropriate handling should take place
                common.notifier.error("Failed to find databases setup for Nancy", getErrorHeader(err));
                return [];
            });
        }

    }

})(window.angular, _);