$(document).ready(function () {

    var loc = window.location.href;
    console.log(loc);
    if (loc == "http://localhost:60764/Account?register=true") {
        console.log("?register=true");
        $('#register-form').toggleClass('show-form');
        $('#login').animate({
            right: "-1000px"
        }, 500, "swing");
    } else if (loc == "http://localhost:60764/Account?login=true") {
        $('#login').animate({
            right: "0px"
        }, 500, "swing");
    }


    $('#signup').on('click', function () {
        console.log("signup");
        $('#register-form').toggleClass('show-form');
        $('#login').animate({
            right: "-1000px"
        }, 500, "swing");
    });

    $('#signin').on('click', function () {
        console.log("signin");
        $('#register-form').toggleClass('show-form');
        $('#login').animate({
            right: "0px"
        }, 500, "swing");
    });

    $('.url-value').on('click', function () {
        console.log(document.getElementById("hidden-url-value").value);
    });

    $('#ddown').on('click', function () {
        $('.dropdown-menu').toggleClass('show');
    });

    //kad se klikne na features
    $('a[href="#features"]').on('click', function () {
        $('html, body').animate({
            scrollTop: $('#features').offset().top + 150
        }, 500);
    });

    //kad se klikne na services
    $('a[href="#pricing"]').on('click', function () {
        console.log("Services");
        $('html, body').animate({
            scrollTop: $('#pricing').offset().top + 150
        }, 500);
    });

    //kad se klikne na about
    $('a[href="#about"]').on('click', function () {
        $('html, body').animate({
            scrollTop: $('#about').offset().top + 150
        }, 500);
    });
});

function copyLink(arg) {
    console.log(arg);

    var object = document.getElementById(arg);

    console.log(object);
    object.select();
    document.execCommand("Copy");
    alert("Link copied to your clipboard.");
}