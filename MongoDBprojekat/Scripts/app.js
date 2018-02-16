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
        $('#register-form').css({
            marginLeft: '-500px'
        });
        $('#login').animate({
            right: "0px"
        }, 500, "swing");
    }


    $('#signup').on('click', function () {
        console.log("signup");
        $('#register-form').toggleClass('show-form');
        $('.show-form').css({
            marginLeft: '897px !important'
        });
        $('#login').animate({
            right: "-1000px"
        }, 500, "swing");
    });

    $('#signin').on('click', function () {
        console.log("signin");
        $('#register-form').toggleClass('show-form');
        $('#register-form').css({
            marginLeft: '-500px'
        });
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
    $('a[href="#features"]').on('click', function (e) {
        var location = this.href.replace(/#/gi, '');
        console.log(location);
        $('html, body').animate({
            scrollTop: $('#features').offset().top + 150
        }, 500);

        e.preventDefault();
    });

    //kad se klikne na services
    $('a[href="#pricing"]').on('click', function (e) {
        var location = this.href.replace(/#/gi, '');
        console.log(location);
        $('html, body').animate({
            scrollTop: $('#pricing').offset().top + 150
        }, 500);

        e.preventDefault();
    });

    //kad se klikne na about
    $('a[href="#about"]').on('click', function (e) {
        var location = this.href.replace(/#/gi, '');
        location.pathname = location;
        console.log(location);
        $('html, body').animate({
            scrollTop: $('#about').offset().top + 150
        }, 500);

        e.preventDefault();
    });

    //form processing - login
    $('#login-form').submit(function (event) {
        console.log("odradjeno");

        var data = {
            'username': $('input[name="username"]').val(),
            'password': $('input[name="password"').val()
        };

        console.log(data);

        $.ajax({
            type: 'POST',
            url: '/Account/Login',
            data: data,
            dataType: 'json',
            encode: true
        }).done(function (data) {
            console.log(data);
            document.location.href = "/Account";
            });

        event.preventDefault();
    });

    //form processing - register
    
});

function copyLink(arg) {
    console.log(arg);

    var object = document.getElementById(arg);

    console.log(object);
    object.select();
    document.execCommand("Copy");
    alert("Link copied to your clipboard.");
}