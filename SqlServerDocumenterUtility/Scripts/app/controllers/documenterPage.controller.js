/*
 * Documenter Page Controller
 * @namespace Controllers
 */
(function (angular) {
    'use strict';

    documenterPageController.$inject = ['$scope', '$timeout', 'documenterService', 'nancyApiService', 'commonService'];
    angular.module('sqlDocumenterApp').controller('documenterPageController', documenterPageController);

    /*
     * @namespace DocumenterPageController
     * @description Controller setup for the _Layout markup
     * @memberOf Controllers
     */
    function documenterPageController(ngScope, ngTimeout, documenterService, nancyApiService, common) {
        /* jshint validthis: true */
        var vm = this;

        //properties
        vm.databases = null;
        vm.connection = null;
        vm.tables = null;
        vm.table = null;
        vm.allowNancy = false;
        vm.useNancy = false;

        //watches
        ngScope.$watch(function () {
            return vm.connection;
        }, function (val) {
            if (val) {
                vm.allowNancy = val.allowNancy;
                vm.useNancy = false;
                nancyApiService.useNancy(false);
                getTables();
            }
            else {
                vm.tables = null;
                vm.table = null;
                vm.allowNancy = false;
                vm.useNancy = false;
            }
        });

        ngScope.$watch(function () {
            return vm.useNancy;
        }, function (val) {
            nancyApiService.useNancy(val);
            getTables();
            if (val) {
                common.notifier.info("You're now using Nancy!", "Aren't You Fancy :-)");
            }
        });

        init();
        return;

        //functions
        function init() {
            ngTimeout(function () {
                common.appRoot = vm.appRootPath;
                nancyApiService.setNancyApiPath(vm.nancyPath);
                getDatabases();
            }, 1);
        }

        function getDatabases() {
            documenterService.getDatabases().
                then(function (data) {
                    vm.databases = data;
                });
        }

        function getTables() {
            if (angular.isDefined(vm.connection) && vm.connection) {
                documenterService.getTables(vm.connection).
                    then(function (data) {
                        vm.tables = data;
                        vm.table = '';
                    });
            }
        }
    }

})(window.angular);