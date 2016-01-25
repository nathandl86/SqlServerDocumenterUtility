/// <binding AfterBuild='default' Clean='cleaner' ProjectOpened='lint' />

//For debugging gulp task: http://s-a.github.io/iron-node/ & https://github.com/s-a/iron-node/blob/master/docs/DEBUG-NODEJS-COMMANDLINE-APPS.md

var gulp = require('gulp'),
    plugins = require('./gulp/plugins.js');

gulp.task('cleaner', getTask('clean'));
gulp.task('lint', getTask('lint-scripts'));
gulp.task('optimize-js', getTask('optimize-scripts'));
gulp.task('optimize-css', getTask('optimize-css'));
gulp.task('optimize-ng-templates', getTask('optimize-ng-templates'));

gulp.task('package-resources', [
    'cleaner',
    'optimize-ng-templates',
    'optimize-js',
    'optimize-css'
]);

gulp.task('default', ['lint', 'package-resources']);

//currently the default task is configured to run after a build. If the 
// the watcher task is part of that process. The build gets stuck in 
// process and cannot complete. Therefore, this task has to be started
// manually for when you're running the application with the optimized
// resources being targetted.
gulp.task('watcher', getTask('watch', watchTrigger));

//helper functions
function getTask(filename, callback) {
    return require('./gulp/tasks/' + filename + '.js')(gulp, plugins, callback);
}

function watchTrigger() {
    gulp.start('lint');
    gulp.start('package-resources');
}
