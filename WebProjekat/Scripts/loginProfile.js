$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data === 'ADMINISTRATOR') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href="korisnici.html"> Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajProdavca.html"> Kreiraj prodavca</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="potvrdaManifestacija.html"> Potvrda manifestacija</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
                PrikaziProfil();
            }
            else if (data === 'KUPAC') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href="register.html"> Moje karte</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
                PrikaziProfil();
            }
            else if (data === 'PRODAVAC') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href="prodavacManifestacije.html"> Moje manifestacije</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajManifestaciju.html"> Kreiraj manifestaciju</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
                PrikaziProfil();
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
    
    function PrikaziProfil() {
        $.ajax({
            url: '/korisnik',
            method: 'GET',
            success: function (data) {
                console.log(data.KorisnickoIme);
                var datum = new Date(data.DatumRodjenja);
                var mesec = datum.getMonth() + 1;
                var eachrow = '<form id="profilForma">'
                                + '<table border="1">'
                                    + "<tr>"
                                        + "<td>" + "Korisnicko ime:" + "</td>"
                                        + "<td>" + `<input type="text" id="korIme" name="korisnickoime" value="${data.KorisnickoIme}" />` + "</td>"
                                    + "</tr>"
                                    + "<tr>"
                                        + "<td>" + "Ime:" + "</td>"
                                        + "<td>" + `<input type="text" id="ime" name="ime" value="${data.Ime}" />` + "</td>"
                                    + "</tr>"
                                    + "<tr>"
                                        + "<td>" + "Prezime:" + "</td>"
                                        + "<td>" + `<input type="text" id="prezime" name="prezime" value="${data.Prezime}" disabled />` + "</td>"
                                    + "</tr>"
                                    + '<tr>'
                                        + '<td>Pol:</td>'
                                        + '<td>'
                                            + '<input type="radio" id="muski" name="pol" checked value="Muski">'
                                            + '<label for="muski">Musko</label>'
                                            + '<input type="radio" id="zenski" name="pol" value="Zenski">'
                                            + '<label for="zenski">Zensko</label><br>'
                                        + '</td>'
                                    + '</tr>'
                                    + "<tr>"
                                        + "<td>" + "Datum rodjenja:" + "</td>"
                                        + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                                    + "</tr>"
                                    + "<tr>"
                                        + '<td colspan="2">' + `<input type="button" id="btnProfil" value="Sacuvaj izmene" data-id="${data.Id}" onclick="izmeniProfil(this)" />` + "</td>"
                                    + "</tr>"
                            + "</table>"
                            + "</form>";
                $('#profilKorisnika').append(eachrow);
            }
        });
    }
});

function izmeniProfil(profil) {
    $(document).on('click', '#btnProfil', function () {
        $.ajax({
            url: '/korisnik',
            method: 'PUT',
            data: {
                Id : $(profil).attr("data-id"),
                KorisnickoIme: $('#korIme').val(),
                Ime: $('#ime').val(),
                Prezime: $('#prezime').val()
            },
            success: function () {
                console.log("USPESNA POSLATI PODACI AJAXOM");
            },
            error: function (jqXHR) {
                alert("ERROR");
            }
        });
    });
}






