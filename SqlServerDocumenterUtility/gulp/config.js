
//Module for common configuration values used between tasks
module.exports = {
    ngModuleName: 'sqlDocumenterApp',
    paths: {
        jsSrc: 'Scripts/**/*.js',
        jsVendor: 'Scripts/vendor/**/*.js',
        jsMinVendor: 'Scripts/vendor/**/*.min.js',
        cssSrc: 'Content/Site.css',
        cssVendor: 'Content/**/*.min.css',
        ngPath: 'Scripts/app/',
        ngTemplates: 'Scripts/app/views/',
        output: 'dist/'
    },
    names: {
        src: 'all',
        vendor: 'vendor',
        ngTemplates: 'templates.js'
    }
};