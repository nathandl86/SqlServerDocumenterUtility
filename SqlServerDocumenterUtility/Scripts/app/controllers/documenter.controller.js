/*
 * Documenter Controller
 * @namespace Controllers
 */
(function (angular, _) {
    'use strict';

    controller.$inject = ['$scope', 'NgTableParams', 'documenterService', 'extendedPropertyService', 'propertyModelBuilderService'];
    angular.module('sqlDocumenterApp').controller('documenterController', controller);

    /*
     * @namespace DocumenterController
     * @description Controller for the Documenter Directive
     * @memberOf Controllers
     */
    function controller(ngScope, NgTableParams, documenterService, propertyService, modelBuilder) {
        /* jshint validthis: true */
        var vm = this;

        //properties
        vm.table = null;
        vm.connection = null;
        vm.columns = [];
        vm.column = null;
        vm.dropdownColumns = [];
        vm.properties = [];
        vm.savedTableProperty = null;
        vm.savedColumnProperty = null;
        vm.tableParams = new NgTableParams({ count: 10 });
        vm.columnParams = new NgTableParams({ count: 10 });
        vm.selectedTableProperty = null;
        vm.selectedColumnProperty = null;

        //methods
        vm.getTableText = getTableText;
        vm.isTableValid = isTableValid;
        vm.editTableProperty = editTableProperty;
        vm.editColumnProperty = editColumnProperty;
        vm.removeTableProperty = removeTableProperty;
        vm.removeColumnProperty = removeColumnProperty;

        //watches
        ngScope.$watch(function () {
            return vm.connection;
        }, selectedDatabaseChanged);

        ngScope.$watch(function () {
            return vm.table;
        }, selectedTableChanged);

        ngScope.$watch(function () {
            return vm.savedTableProperty;
        }, savedTableTrigger);

        ngScope.$watch(function () {
            return vm.savedColumnProperty;
        }, savedColumnTrigger);

        return;

        //functions
        /*
         * @name getTableText
         * @description Determines the table text to display in the table type-ahead control
         * @returns {String}
         * @memberOf DocumenterController
        */
        function getTableText() {
            if (angular.isDefined(vm.table) && vm.table && angular.isDefined(vm.table.tableId)) {
                return vm.table.schemaName + '.' + vm.table.tableName;
            }
        }

        /*
         * @name isTableValid
         * @description Determines if the selected table from the type-ahead is valid
         * @returns {boolean}
         * @memberOf DocumenterController
         */
        function isTableValid() {
            if (angular.isDefined(vm.table) && vm.table) {
                return vm.table.tableId;
            }
            return false;
        }

        /*
         * @name editTableProperty
         * @description Triggered from selected row in table properties grid
         * @param {Object} row
         * @memberOf DocumenterController
        */
        function editTableProperty(row) {
            if (angular.isDefined(row) && row !== null) {
                vm.selectedTableProperty = {
                    objectId: row.tableId,
                    name: row.name,
                    description: row.text
                };
            }
        }

        /*
         * @name editColumnProperty
         * @description Triggered from selected row in the column properties grid
         * @param {Object} row
         * @memberOf DocumenterController
         */
        function editColumnProperty(row) {
            if (angular.isDefined(row) && row !== null) {
                var column = _.find(vm.columns, function (c) {
                    return c.columnId === row.columnId;
                });

                if (angular.isDefined(column)) {
                    vm.column = column;
                }

                vm.selectedColumnProperty = {
                    objectId: row.columnId,
                    name: row.name,
                    description: row.text
                };
            }
        }

        /*
         * @name removeTableProperty
         * @description Triggered from selecting to remove an item from the 
         *      table properties grid
         * @param {Object} row
         * @memberOf DocumenterController
        */
        function removeTableProperty(row) {
            if (angular.isDefined(row) && row !== null) {
                propertyService.deleteProperty(row, vm.connection).
                                    then(function () {
                                        getProperties();
                                    });
            }
        }

        /*
         * @name remvoeColumnProperty
         * @description Triggered from selecting to remove an item from the
         *      column properties grid
         * @param {ObjectId} row
         * @memberOf DocumenterController
         */
        function removeColumnProperty(row) {
            if (angular.isDefined(row) && row !== null) {
                propertyService.deleteProperty(row, vm.connection).
                                    then(function () {
                                        getProperties();
                                    });
            }
        }

        function getColumns() {
            if (vm.table && vm.connection) {
                documenterService.getColumns(vm.table.tableId, vm.connection).
                    then(function (data) {
                        vm.columns = data;
                        var all = { "columnName": "All" };
                        vm.dropdownColumns = [all].concat(vm.columns);
                        vm.column = all;
                    });
            }
        }

        function getProperties() {
            if (vm.table && vm.connection) {
                propertyService.getProperties(vm.table.tableId, vm.connection).
                    then(function (data) {
                        vm.properties = data;
                        vm.tableParams.settings({
                            dataset: getTableProperties()
                        });
                        vm.columnParams.settings({
                            dataset: getColumnProperties()
                        });
                    });
            }
        }

        function getTableProperties() {
            if (angular.isUndefined(vm.properties) || vm.properties.length === 0) {
                return [];
            }
            else {
                return _.filter(vm.properties, function (p) {
                    return p.columnId === null || p.columnId <= 0;
                });
            }
        }

        function getColumnProperties() {
            if (angular.isUndefined(vm.properties) || vm.properties.length === 0) {
                return [];
            }
            else {
                return _.filter(vm.properties, function (p) {
                    return p.columnId !== null && p.columnId > 0;
                });
            }
        }

        function selectedDatabaseChanged(val) {
            if (angular.isDefined(val)) {
                vm.connection = val;
            }
            else {
                vm.properties = [];
                vm.columns = [];
                vm.dropdownColumns = [];
                vm.column = null;
                vm.page.table = null;
                vm.page.connection = null;
            }
            vm.savedTableProperty = null;
            vm.savedColumnProperty = null;
        }

        function selectedTableChanged(val) {
            if (angular.isDefined(val) && val && angular.isDefined(val.tableId)) {
                vm.table = val;
                getColumns();
                getProperties();
            }
            else {
                vm.column = null;
                vm.columns = [];
                vm.dropdownColumns = [];
                vm.properties = [];
            }
            vm.savedTableProperty = null;
            vm.savedColumnProperty = null;
        }

        function savedTableTrigger(val) {
            if (angular.isDefined(val) && val && angular.isDefined(val.objectId)) {
                var model = createBaseModel(val);

                var preExisting = _.find(vm.properties, function (p) {
                    return p.tableId === vm.table.tableId && p.name === model.name && (p.columnId === null || p.columnId <= 0);
                });

                saveModel(model, preExisting);
            }
        }

        function savedColumnTrigger(val) {
            if (angular.isDefined(val) && val && angular.isDefined(val.objectId) && angular.isDefined(vm.column)) {
                var model = createBaseModel(val);
                var columnId = vm.column.columnId;
                model.columnId = columnId;
                model.columnName = vm.column.columnName;

                var preExisting = _.find(vm.properties, function (p) {
                    return p.columnId === columnId && p.name === model.name;
                });

                saveModel(model, preExisting);
            }
        }

        function saveModel(model, isPreExisting) {
            if (typeof isPreExisting === "undefined") {
                propertyService.addProperty(model, vm.connection).
                    then(function () {
                        getProperties();
                    });
            }
            else {
                propertyService.updateProperty(model, vm.connection).
                    then(function () {
                        getProperties();
                    });
            }
        }

        function createBaseModel(val) {
            if (angular.isDefined(vm.table)) {
                return modelBuilder.create({
                    propertyName: val.propertyName,
                    propertyValue: val.propertyValue,
                    schemaId: vm.table.schemaId,
                    schemaName: vm.table.schemaName,
                    tableId: vm.table.tableId,
                    tableName: vm.table.tableName
                });
            }
        }
    }

})(window.angular, _);