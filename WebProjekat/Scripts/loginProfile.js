$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data === 'ADMINISTRATOR') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href = "register.html"> Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
            else if (data === 'KUPAC') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href = "register.html"> Moje karte</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
            else if (data === 'PRODAVAC') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href = "register.html"> Moje manifestacije</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }

        }
    });

    $('#regLogoutKartica').click(function () {
        if ($('#regLogoutKartica').text() === 'Odjavi se') {
            $.ajax({
                url: '/account/logout',
                method: 'GET',
                success: function () {
                    window.location.href = "index.html";
                }
            });
        }
    });

    $('#btnLog').click(function () {
        $.ajax({
            url: '/account/login',
            method: 'POST',
            data: {
                KorisnickoIme : $('#korIme').val(),
                Lozinka : $('#lozinka').val()
            },
            success: function () {
                window.location.href = 'index.html';
            },
            error: function (jqXHR) {
                console.log(jqXHR);
            }
        });
    });

});