$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data === 'ADMINISTRATOR') {
                var kartice = '<li class="nav-item"><a class="nav-link" href = "register.html"> Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
            }
            else if (data === 'KUPAC') {
                var kartice = '<li class="nav-item"><a class="nav-link" href = "register.html"> Moje karte</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
            }
            else if (data === 'PRODAVAC') {
                var kartice = '<li class="nav-item"><a class="nav-link" href = "register.html"> Moje manifestacije</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
            }

        }
    });

    $("#btnReg").click(function () {
        $.ajax({
            url: '/account/register',
            method: 'POST',
            data: {
                KorisnickoIme: $('#korIme').val(),
                Lozinka: $('#lozinka').val(),
                Ime: $('#ime').val(),
                Prezime: $('#prezime').val(),
                Pol: $('input[name="pol"]:checked').val(),
                DatumRodjenja: $('#datumRodj').val(),
                Uloga: $('#uloga').val(),
                SveMojeKarteBezObziraNaStatus: $('#sveMojeKarteBezObziraNaStatus').val(),
                BrojSakupljenihBodova: $('brojSakupljenihBodova').val(),
                IsDeleted: $('isDeleted').val()
            },
            success: function () {
                console.log("USPESNA POSLATI PODACI AJAXOM");
                window.location.href = "index.html";
            },
            error: function (jqXHR) {
                alert("ERROR");
            }

        });
    });




});