/*
 * PropertyModel
 * @namespace global
 * @description client-side model for an extended property
 */
function PropertyModel(details) {
    this.name = getValue("propertyName");
    this.text = getValue("propertyValue");
    this.schemaId = getValue("schemaId");
    this.schemaName = getValue("schemaName");
    this.tableId = getValue("tableId");
    this.tableName = getValue("tableName");
    this.columnId = getValue("columnId");
    this.columnName = getValue("columnName");

    
    function getValue(propName) {
        return isUndefinedOrNull(propName) ? null : details[propName];
    }

    function isUndefinedOrNull(propName) {
        if (typeof details !== "undefined" &&
            details !== null &&
            typeof details[propName] !== "undefined") {
            return false;
        }
        return true;
    }
}
/*
 * PropertySuggestions
 * @namespace global
 * @description Property suggestion factory to get suggestions
 *      based upon the type passed in.
 */
function PropertySuggestions() {
    var commonSuggestions = ['Description', 'Dependencies'];
    var tableSuggestions = ['Module'];
    var columnSuggestions = ['Aliases'];

    this.get = get;

    return;
    
    function get(type) {
        switch ((type || '').toLowerCase()) {
            case 'table':
                return getTable();
            case 'column':
                return getColumn();
            default:
                return getSuggestions(commonSuggestions);
        }
    }

    function getTable() {
        var suggestions = commonSuggestions.concat(tableSuggestions);
        return getSuggestions(suggestions);
    }

    function getColumn() {
        var suggestions = commonSuggestions.concat(columnSuggestions);
        return getSuggestions(suggestions);
    }

    function getSuggestions(suggestions) {
        suggestions.sort();
        return suggestions;
    }
}
/*
 * ScollTop
 * @namespace global
 * @description Handler for scrolling for the app that will 
 *  hide and show a button enabling the user to quickly 
 *  be scrolled back to the top of the page.
 */
var ScrollTop = (function (me) {
    var offset = 60, // minimum scroll height for the button to show
        offsetOpacity = 250, // scroll height at which to dim the opacity of the button
        scrollDuration = 700;

    $(window).scroll(onScroll);
    $('.scroll-top').on('click', onScrollTopClicked);

    function onScroll(ev) {
        var scrollPos = $(window).scrollTop();
        var elem = $('.scroll-top');

        if (scrollPos < offset) {
            elem.removeClass('scroll-top-visible, scroll-top-faded').hide();
        }
        else if (scrollPos > offsetOpacity) {
            elem.removeClass('scroll-top-visible').addClass('scroll-top-faded').show();
        }
        else {
            elem.removeClass('scroll-top-faded').addClass('scroll-top-visbile').show();
        }
    }

    //click handler for the scroll top button
    function onScrollTopClicked(ev) {
        var anchor = $(this);
        animateScrollTop(anchor);
        ev.preventDefault();
    }

    //function handling the animated scrolling
    function animateScrollTop(anchor) {
        $('html, body').stop().animate({
            scrollTop: $(anchor.attr('href')).offset().top - 30
        }, scrollDuration);
    }

})(ScrollTop);

/*
 * SectionLoader
 * @namespace global
 * @description Handler for dynamically showing additional content
 *      on the application page that isn't initally rendered when 
 *      the page laods.
 */
var SectionLoader = (function (me) {
    $('.page-scroll a').on('click', onMenuItemClick);

    var contentContainers = [];

    //hanlder for the click of a nav menu link
    function onMenuItemClick(ev) {
        var anchor = $(this);
        loadPartial(anchor);
        animateScroll(anchor);
        ev.preventDefault();
    }

    //animates the downward scroll to the section
    function animateScroll(anchor) {
        $('html, body').stop().animate({
            scrollTop: $(anchor.attr('href')).offset().top - 30
        }, 700);
    }

    //loads a partial view by sending a get request to the MVC
    // controller based on the url data attribute of the clicked
    // menu link
    function loadPartial(anchor) {
        var section = $(anchor.attr('href'));

        //makes the call to get additional conent and appends it to the section
        // matching the href attribute of the menu link
        if (section.hasClass("lazy-load") && section.find('.section').length <= 0) {
            var url = anchor.data('url');
            $.get(url, function (data) {
                section.append(data);
                
                //evalutates the content containers if there is additional 
                // dynamic content that should be rendered in the new content.
                if (contentContainers.length > 0) {
                    for (var i = 0, len = contentContainers.length; i < len; i++ ){
                        var item = contentContainers[i];
                        if (section.attr('id').toLowerCase() === item.name.toLowerCase()) {
                            $(item.selector).append(item.data);
                        }
                    }
                }
            });
        }        
    }

    //gets the markdown's compiled html file and creates a content container item
    // so that when the about link is clicked, the markdown html can be placed into it.
    var markdownContainer = $('#markdownLocation');
    $.get(markdownContainer.data('url'), function (data) {
        contentContainers.push({ 
            name: "about", 
            selector: "#markdownDestination",
            data: data
        });
    });
})(SectionLoader);
/*
 * Angular Module Declaration
 * @namespace App
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp', [
        'ui.bootstrap',
        'ngTable'
    ]);

})(window.angular);
/*
 * Angular Value Services
 * @namespace Values
 * @description setup for the default values used for api interaction
 */
(function (angular) {
    'use strict';

    var app = angular.module('sqlDocumenterApp');

    app.value("baseUrl", "");
    app.value("nancyUrl", "nancy/");

})(window.angular);
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
/*
 * Custom Dropdown Controller
 * @namespace Controllers
 */
(function (angular) {
    'use strict';

    dropdownController.$inject = [];
    angular.module('sqlDocumenterApp').controller('dropdownController', dropdownController);

    /*
     * @namespace CustomDropdownController
     * @description Controller for the custom drodown directive
     * @memberOf Controllers
     */
    function dropdownController() {
        /* jshint validthis: true */
        var vm = this,
            defaultText = vm.textCaption || '-- Select an Item --';

        //properties
        vm.disabled = vm.disabled || false;
        vm.open = false;
        vm.cssClass = vm.cssClass || "";
        vm.text = vm.text || defaultText;

        //methods
        vm.isOpen = isOpen;
        vm.isDisabled = isDisabled;
        vm.select = select;
        vm.getText = getText;        
        vm.getSelectedText = getSelectedText;

        /*
         * @name getText
         * @description Determines the text string from an item
         * @param {Object} item
         * @returns {String}
         * @memberOf CustomDropdownController
         */
        function getText(item) {
            if (angular.isDefined(item) && item) {
                return item[vm.textPath] || item.toString();
            }
        }

        /*
         * @name getSelectedText
         * @description Gets the text of the selected item
         * @returns {String}
         * @memberOf CustomDropdownController
        */
        function getSelectedText() {
            var selectedItemText = getText(vm.selected);
            return selectedItemText || vm.text;
        }

        /*
         * @name select
         * @description selection handler from the dropown when an item is selected
         * @param {Object} $e Event args
         * @param {Object} data Selected item data
         * @memberOf CustomDropdownController
         */
        function select($e, data) {
            vm.selected = data;
            vm.text = vm.selected[vm.textPath] || data.toString();
        }

        /*
         * @name isDisabled
         * @description Determines if the dropdown should be disabled
         * @returns {boolean}
         * @memberOf CustomDropdownController
         */
        function isDisabled() {
            if (vm.disabled) return true;
            if (vm.items === null) return true;
            if (vm.items.length === 0) return true;
            return false;
        }

        /*
         * @name isOpen
         * @description Determines if the dropdown is open
         * @returns {boolean}
         * @memberOf CustomDropdownController
         */
        function isOpen() {
            return vm.open;
        }
    }

})(window.angular);
/*
 * Property Form Controller
 * @namespace Controllers
 */
(function (angular) {
    'use strict';/* jshint validthis: true */

    propertyFormController.$inject = ['$scope', '$timeout', 'propertySuggestionsService'];
    angular.module('sqlDocumenterApp').controller('propertyFormController', propertyFormController);

    /*
     * @namespace PropertyFormController
     * @description Controller for the property form directive
     * @memberOf Controllers
     */
    function propertyFormController(ngScope, ngTimeout, suggestionService) {
        var vm = this;

        //properties
        vm.name = '';
        vm.description = null;
        vm.suggestions = [];

        //methods
        vm.submit = submit;
        vm.clear = clear;
        vm.isFormDisabeld = isFormDisabeld;

        //watch(es)
        ngScope.$watch(function () {
            return vm.property;
        }, propertyChanged);

        init();
        return;

        //functions
        function init() {
            vm.suggestions = suggestionService.get(vm.type);
        }

        /*
         * @name submit
         * @description submits changes to existing or creation of a new extended property
         * @memberOf PropertyFormController
         */
        function submit() {
            vm.saved = {
                objectId: vm.objectId,
                propertyName: vm.name,
                propertyValue: vm.description
            };
        }

        /*
         * @name clear
         * @description clears the form
         * @memberOf PropertyFormController
         */
        function clear() {
            vm.name = '';
            vm.description = null;
            vm.property = null;
        }

        /*
         * @name isFormDisabled
         * @description Determining if the property form should be enabled or not
         * @returns {boolean}
         * @memberOf PropertyFormController
         */
        function isFormDisabeld() {
            return angular.isUndefined(vm.objectId) || vm.objectId === null || vm.objectId === '';
        }

        function propertyChanged(val) {
            if (angular.isDefined(val) && val) {
                val = JSON.parse(val);
                vm.name = val.name;
                vm.description = val.description;
                vm.objectId = val.objectId;
            }
        }        

    }

})(window.angular);
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
/*
 * Custom Dropdown
 * @namespace Directives
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp').directive('customDropdown', customDropdown);

    /*
    * @namespace CustomDropdown
    * @description Directive encapsulating a common dropdown
    * @memberOf Directives
    */
    function customDropdown() {

        var directive = {
            restrict: 'E',
            replace: true,
            require: '?ngModel',
            scope: {
                items: '=',
                text: '=',
                textPath: '@?',
                textCaption: '@?',
                selected: '=',
                disabled: '=',
                cssClass: '@?'
            },
            templateUrl: '/Scripts/app/views/dropdown.directive.html',
            controller: 'dropdownController',
            controllerAs: 'drop',
            bindToController: true,
            link: angular.noop
        };

        return directive;
    }


})(window.angular);
/*
 * Property Form
 * @namespace Directives
 */
(function (angular) {
    'use strict';

    angular.module('sqlDocumenterApp').directive('propertyForm', propertyFormDirective);

    /*
     * @namespace PropertyForm
     * @description Directive encapsulating the functionality for creating properties
     * @memberOf Directives
     */
    function propertyFormDirective() {
        var directive = {
            restrict: 'EA',
            replace: false,
            scope: {
                objectId: '@',
                property: "@",
                type: '@',
                saved: '=',
            },
            templateUrl: '/Scripts/app/views/propertyForm.directive.html',
            controller: 'propertyFormController',
            controllerAs: 'propertyForm',
            bindToController: true
        };

        return directive;
    }

})(window.angular);
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
            void 0;
        }
    }

})(window.angular);
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
/*
 * Property Suggestion Service
 * @namespace Services
 * @description Service for getting property suggestions that differ depending on if adding/updating
 *      property on a table or column
 */
(function (angular, _) {
    'use strict';

    angular.module('sqlDocumenterApp').service('propertySuggestionsService', PropertySuggestions);

})(window.angular, _);