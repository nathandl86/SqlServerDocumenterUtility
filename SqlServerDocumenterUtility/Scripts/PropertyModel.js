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