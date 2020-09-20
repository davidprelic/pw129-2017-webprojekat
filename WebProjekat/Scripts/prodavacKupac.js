$(document).ready(function () {

    $('.header').height($(window).height());

    var uloga;

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            uloga = data;
            if (data === 'ADMINISTRATOR') {
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
            else if (data === 'PRODAVAC') {
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
            else {
                window.location.href = "index.html";
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

    var getUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;
        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    };

    var idParametar = getUrlParameter('id');

    function PrikaziProfil() {
        $.ajax({
            url: `/korisnik/${idParametar}`,
            method: 'GET',
            success: function (data) {
                var datum = new Date(data.DatumRodjenja);
                var dan = datum.getDate();
                var mesec = datum.getMonth() + 1;
                var godina = datum.getFullYear();

                var eachrow = '<form id="profilForma">'
                    + '<table border="1">'
                    + "<tr>"
                    + "<td>" + "Korisnicko ime:" + "</td>"
                    + "<td>" + `<input type="text" id="korIme" name="korisnickoime" value="${data.KorisnickoIme}" disabled />` + "</td>"
                    + "</tr>"
                    + "<tr>"
                    + "<td>" + "Ime:" + "</td>"
                    + "<td>" + `<input type="text" id="ime" name="ime" value="${data.Ime}" disabled />` + "</td>"
                    + "</tr>"
                    + "<tr>"
                    + "<td>" + "Prezime:" + "</td>"
                    + "<td>" + `<input type="text" id="prezime" name="prezime" value="${data.Prezime}" disabled />` + "</td>"
                    + "</tr>"
                    + '<tr>'
                    + '<td>Pol:</td>';
                if (data.Pol === 0)
                    eachrow += '<td>Musko</td>';
                else
                    eachrow += '<td>Zensko</td>';

                    eachrow += '</tr>'
                    + "<tr>"
                    + "<td>" + "Datum rodjenja:" + "</td>"
                        + "<td>" + dan + '/' + mesec + '/' + godina + "</td>"
                    + "</tr>";
                
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
                        + "</table>"
                        + "</form>";
                $('#profilKupca').append(eachrow);
            }
        });
    }
});








