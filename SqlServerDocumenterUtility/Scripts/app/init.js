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