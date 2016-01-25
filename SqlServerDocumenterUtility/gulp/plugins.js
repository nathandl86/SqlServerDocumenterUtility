
//module for the plugins shared amongst the tasks
module.exports = {
    config: require('./config.js'),
    filter: require('gulp-filter'),
    jshint: require('gulp-jshint'),
    stylish: require('jshint-stylish'),
    notify: require('gulp-notify'),
    watch: require('gulp-watch'),
    del: require('del'),
    concat: require('gulp-concat'),
    concatCss: require('gulp-concat-css'),
    uglify: require('gulp-uglify'),
    minifyCss: require('gulp-minify-css'),
    rename: require('gulp-rename'),
    stripDebug: require('gulp-strip-debug'),
    ngTemplateCache: require('gulp-angular-templatecache')
};