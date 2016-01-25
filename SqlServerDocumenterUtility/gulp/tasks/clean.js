//module to clean directories 
module.exports = function (gulp, plugins) {
    return function () {
        var js = plugins.config.paths.output + "*.js",
            css = plugins.config.paths.output + "*.css";

        return plugins.del([js, css]);
    }
}