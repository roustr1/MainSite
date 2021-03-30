//const gulp = require("gulp");
//const sass = require("gulp-sass");
//const sourcemaps = require("sourcemaps");
//const watch = require("gulp-watch");

/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    sass = require("gulp-sass"); // добавляем модуль sass

var paths = {
    webroot: "./wwwroot/"
};
// регистрируем задачу для конвертации файла scss в css
gulp.task("sass-lib", function () {
    return gulp.src(paths.webroot +'css/materialize/sass/materialize.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.webroot + 'css/materialize/sass'));
});

gulp.task("sass-main", function () {
    return gulp.src(paths.webroot + 'css/mainSite.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.webroot + 'css/'));
});

gulp.task("sass-admin", function () {
    return gulp.src(paths.webroot + 'css/adminSite.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.webroot + 'css/'));
});