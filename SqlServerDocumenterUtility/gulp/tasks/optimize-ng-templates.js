//module to bundle and minify the angular template views
module.exports = function (gulp, plugins) {
    return function () {
        var path = plugins.config.paths.ngTemplates,
            srcMask = path + '*.html';

        return gulp.src(srcMask).
            pipe(plugins.ngTemplateCache(plugins.config.names.ngTemplates, {
                root: path,
                module: plugins.config.ngModuleName
            })).
            pipe(gulp.dest(plugins.config.paths.output));
    }
}
