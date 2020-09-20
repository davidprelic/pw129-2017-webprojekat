$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data !== 'PRODAVAC') {
                window.location.href = "index.html";
            }
            else {
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
            }
        }
    });

    $.ajax({
        url: '/rezervisane-karte',
        method: 'GET',
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                var datum = new Date(data[i].DatumVremeManifestacije);
                var mesec = datum.getMonth() + 1;

                var tipKarte = "";

                switch (data[i].Tip) {
                    case 0:
                        tipKarte = "VIP";
                        break;
                    case 1:
                        tipKarte = "REGULAR";
                        break;
                    case 2:
                        tipKarte = "FANPIT";
                }

                var eachKarta = "<tr>"
                    + "<td>" + `<a data-id="${data[i].ManifestacijaID}" onclick="prikaziManif(this)">` + data[i].NazivManif + '</a>' + "</td>"
                    + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                    + "<td>" + data[i].Cena + "</td>"
                    + "<td>" + `<a data-id="${data[i].KupacID}" onclick="prikaziKupca(this)">` + data[i].KorImeKupca + '</a>' + "</td>"
                    + "<td>" + tipKarte + "</td>"
                    + "</tr>";
                $('#tbodyRezKarteProdavca').append(eachKarta);
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
});

function prikaziManif(jednaManif) {
    window.location = "pregledManifestacije.html" + `?id=${$(jednaManif).attr("data-id")}`;
}

function prikaziKupca(kupac) {
    window.location = "prodavacKupac.html" + `?id=${$(kupac).attr("data-id")}`;
}