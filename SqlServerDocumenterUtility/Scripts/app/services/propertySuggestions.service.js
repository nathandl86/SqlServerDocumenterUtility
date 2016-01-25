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