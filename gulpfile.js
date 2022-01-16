var gulp = require("gulp");
var uglify = require("gulp-uglify");
var concat = require("gulp-concat");

const minifyJs = () => {
    return gulp.src(["wwwroot/js/**/*.js"])
        .pipe(uglify())
        .pipe(concat("dutchtreat.min.js"))
        .pipe(gulp.dest("wwwroot/dist/"));
}

const minifyCss = () => {
    return gulp.src(["wwwroot/css/**/*.css"])
        .pipe(uglify())
        .pipe(concat("dutchtreat.min.css"))
        .pipe(gulp.dest("wwwroot/dist/"))
}

exports.minifyJs = minifyJs;
exports.minifyCss = minifyCss;

exports.default = gulp.parallel(minifyJs, minifyCss);