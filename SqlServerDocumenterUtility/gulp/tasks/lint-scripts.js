
//module for lint-ing scripts (ignores vendor scripts)
module.exports = function (gulp, plugins) {
    return function () {
        return gulp.src([plugins.config.paths.jsSrc,
            '!Scripts/_references.js',
            '!' + plugins.config.paths.jsVendor])
        .pipe(plugins.jshint())
        .pipe(plugins.jshint.reporter(plugins.stylish));
    }
}