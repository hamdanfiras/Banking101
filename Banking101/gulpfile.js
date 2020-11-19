const { series, src, dest, watch, task } = require('gulp');
const sass = require('gulp-sass');
const cssnano = require('gulp-cssnano');

function bundleSass() {
    return src("wwwroot/css/style.scss")
        .pipe(sass({ style: 'compressed' }))
        .pipe(cssnano())
        .pipe(dest('wwwroot/css/dist/'));
}

task("sass", bundleSass);

exports.default = series(bundleSass);