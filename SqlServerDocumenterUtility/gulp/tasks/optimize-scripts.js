//module to bundle and minify js files
module.exports = function (gulp, plugins) {
    var vend = plugins.config.paths.jsMinVendor;
    var src = [
        plugins.config.paths.jsSrc,
        '!Scripts/_references.js',
        '!' + plugins.config.paths.jsVendor
    ];

    return function () {
        gulp.src(src).
            pipe(plugins.stripDebug()).
            pipe(plugins.concat(plugins.config.names.src + '.js')).
            pipe(gulp.dest(plugins.config.paths.output)).
            pipe(plugins.rename(plugins.config.names.src + '.min.js')).
            pipe(plugins.uglify({ mangle: false })).
            pipe(gulp.dest(plugins.config.paths.output));
    }
}