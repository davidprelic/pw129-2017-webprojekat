$(document).ready(function () {

    $('.header').height($(window).height());

    var uloga;

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            uloga = data;
            if (data === 'ADMINISTRATOR') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href="korisnici.html"> Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="adminKarte.html"> Sve karte</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajProdavca.html"> Kreiraj prodavca</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="potvrdaManifestacija.html"> Potvrda manifestacija</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="komentari.html"> Prikaz komentara</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
                PrikaziProfil();
            }
            else if (data === 'KUPAC') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href="kupacKarte.html"> Moje karte</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
                PrikaziProfil();
            }
            else if (data === 'PRODAVAC') {
                $('#loginDiv').remove();
                var kartice = '<li class="nav-item"><a class="nav-link" href="prodavacManifestacije.html"> Moje manifestacije</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="prodavacKarte.html"> Rezervisane karte</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajManifestaciju.html"> Kreiraj manifestaciju</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="komentari.html"> Prikaz komentara</a></li>';
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
        $("p").remove();
        if ($('#korIme').val().length < 1)
            $('#korIme').after('<p>Unesite korisnicko ime</p>');
        else if ($('#lozinka').val().length < 1)
            $('#lozinka').after('<p>Unesite lozinku</p>');
        else {
            $.ajax({
                url: '/account/login',
                method: 'POST',
                data: {
                    KorisnickoIme: $('#korIme').val(),
                    Lozinka: $('#lozinka').val()
                },
                success: function () {
                    window.location.href = 'index.html';
                },
                error: function (jqXHR) {
                    console.log(jqXHR);
                }
            });
        }
    });

    function pad(str, max) {
        str = str.toString();
        return str.length < max ? pad("0" + str, max) : str;
    }

    function PrikaziProfil() {
        $.ajax({
            url: '/korisnik',
            method: 'GET',
            success: function (data) {
                var datum = new Date(data.DatumRodjenja);
                var dan = pad(datum.getDate(), 2);
                var mesec = pad(datum.getMonth() + 1, 2);
                var godina = datum.getFullYear();

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
                    + "<td>" + `<input type="text" id="prezime" name="prezime" value="${data.Prezime}" />` + "</td>"
                    + "</tr>"
                    + '<tr>'
                    + '<td>Pol:</td>'
                    + '<td>';
                if (data.Pol === 0) {
                    eachrow += '<input type="radio" id="muski" name="pol" checked value="Muski">'
                        + '<label for="muski">Musko</label>'
                        + '<input type="radio" id="zenski" name="pol" value="Zenski">'
                        + '<label for="zenski">Zensko</label><br>'
                        + '</td>'
                }
                else {
                    eachrow += '<input type="radio" id="muski" name="pol" value="Muski">'
                        + '<label for="muski">Musko</label>'
                        + '<input type="radio" id="zenski" name="pol" checked value="Zenski">'
                        + '<label for="zenski">Zensko</label><br>'
                        + '</td>'
                }
                    eachrow += '</tr>'
                    + "<tr>"
                    + "<td>" + "Datum rodjenja:" + "</td>"
                    + "<td>" + `<input type="date" id="datumRodj" name="datumRodj" value="${godina}-${mesec}-${dan}">` + "</td>"
                    + "</tr>";
                if (uloga === "KUPAC") {
                    var tipKor = "";

                    switch (data.TipKorisn.ImeTipa) {
                        case 0:
                            tipKor = "ZLATNI";
                            break;
                        case 1:
                            tipKor = "SREBRNI";
                            break;
                        case 2:
                            tipKor = "BRONZANI";
                    }

                    eachrow += "<tr>"
                        + "<td>" + "Broj sakupljenih bodova:" + "</td>"
                        + "<td>" + `<input type="text" id="brojSakupljenihBodova" name="brojSakupljenihBodova" value="${data.BrojSakupljenihBodova}"  disabled />` + "</td>"
                        + "</tr>"
                        + "<tr>"
                        + "<td>" + "Tip korisnika:" + "</td>"
                        + "<td>" + `<input type="text" id="tipKorisnika" name="tipKorisnika" value="${tipKor}"  disabled />` + "</td>"
                        + "</tr>"
                }

                    eachrow += "<tr>"
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
                Prezime: $('#prezime').val(),
                Pol: $('input[name="pol"]:checked').val(),
                DatumRodjenja: $('#datumRodj').val()
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






