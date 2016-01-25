
//module to setup watches on directories where changes need to
// trigger other tasks (minification, bundling, linting, etc...
// The callback function arg is optional, and is how subsequent
// tasks are triggered.
module.exports = function (gulp, plugins, callback) {
    return function () {
        return plugins.watch([
                plugins.config.paths.ngTemplates + '*.html',
                plugins.config.paths.cssSrc,
                plugins.config.paths.jsSrc
        ], function () {
            if (typeof callback === "function") {
                callback();
            }
        });
    }
}