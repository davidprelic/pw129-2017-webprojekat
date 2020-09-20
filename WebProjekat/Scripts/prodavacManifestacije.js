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
        url: '/manifestacijeprodavca',
        method: 'GET',
        success: function (data) {
            // Sortiranje po datumu
            data.sort(function (a, b) {
                var dateA = new Date(a.DatumVremeOdrzavanja), dateB = new Date(b.DatumVremeOdrzavanja);
                return dateA - dateB;
            });


            for (var i = 0; i < data.length; i++) {
                var datum = new Date(data[i].DatumVremeOdrzavanja);
                var mesec = datum.getMonth() + 1;

                var tipManif = "";

                switch (data[i].Tip) {
                    case 0:
                        tipManif = "KONCERT";
                        break;
                    case 1:
                        tipManif = "FESTIVAL";
                        break;
                    case 2:
                        tipManif = "POZORISTE";
                        break;
                    case 3:
                        tipManif = "SPORT";
                }

                var eachManif = `<div class="col-sm-3 trenutnaManif">`
                    + '<div class="card text-center border-success">'
                    + '<div class="card-header">' + tipManif + '</div>'
                    + '<div class="card-body">'
                    + `<img class="card-img-top" src="${data[i].PosterManifestacije}" data-id="${data[i].Id}" onclick="prikaziManif(this)">`
                    + '<h5 class="card-title">' + data[i].Naziv + "</h5>"
                    + '<p class="card-text">' + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</p>"
                    + `<button type="button" class="izmeniManif btn btn-primary" data-id="${data[i].Id}" onclick="izmeniManif(this)">Izmeni manifestaciju</button>`
                    + '</div>'
                    + '</div>'
                    + "</div>";
                $('.card-deck').append(eachManif);
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

function izmeniManif(jednaManif) {
    window.location = "izmeniManifestaciju.html" + `?id=${$(jednaManif).attr("data-id")}`;
}