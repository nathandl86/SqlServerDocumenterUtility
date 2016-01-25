//module to bundle and minify css files
module.exports = function (gulp, plugins) {
    return function () {
        gulp.src(plugins.config.paths.cssSrc).
            pipe(plugins.concatCss(plugins.config.names.src + '.css')).
            pipe(plugins.rename(plugins.config.names.src + '.min.css')).
            pipe(plugins.minifyCss()).
            pipe(gulp.dest(plugins.config.paths.output));

        gulp.src(plugins.config.paths.cssVendor).
            pipe(plugins.concatCss(plugins.config.names.vendor + '.css')).
            pipe(plugins.rename(plugins.config.names.vendor + '.min.css')).
            pipe(plugins.minifyCss()).
            pipe(gulp.dest(plugins.config.paths.output));
    }
}