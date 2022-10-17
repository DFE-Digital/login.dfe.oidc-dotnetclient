var gulp = require("gulp");
var sass = require("gulp-sass");

gulp.task('site_sass', function () {
    return gulp.src('./sass/**/*.scss')
        .pipe(sass({
            includePaths: 'node_modules'
        }))
        .pipe(gulp.dest('wwwroot/assets/'));
});

gulp.task('govuk_scripts', function () {
    return gulp.src('node_modules/govuk-frontend/govuk/all.js')
        .pipe(gulp.dest('wwwroot/js/govuk-frontend/'));
});

gulp.task('govuk_assets', function () {
    return gulp.src('node_modules/govuk-frontend/govuk/assets/**/*.*')
        .pipe(gulp.dest('wwwroot/assets/'));
});

