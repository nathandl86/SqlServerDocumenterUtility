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