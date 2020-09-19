$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data !== 'ADMINISTRATOR') {
                window.location.href = "index.html";
            }
            else {
                var kartice = '<li class="nav-item"><a class="nav-link" href="korisnici.html"> Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajProdavca.html"> Kreiraj prodavca</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="potvrdaManifestacija.html"> Potvrda manifestacija</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="komentari.html"> Prikaz komentara</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
        }
    });

    $.ajax({
        url: '/prikaz-manifestacije-potvrda',
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

                var eachManif = `<div class="col-sm-3">`
                    + `<div class="card text-center border-success trenutnaManif" data-id="${data[i].Id}" onclick="prikaziManif(this)">`
                                    + '<div class="card-header">' + tipManif + '</div>'
                                        + '<div class="card-body">'
                                            + `<img class="card-img-top" src="${data[i].PosterManifestacije}">`
                                            + '<h5 class="card-title">' + data[i].Naziv + "</h5>"
                                            + '<p class="card-text">' + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</p>"
                                        + '</div>'
                    + '</div>'
                    + `<button type="button" class="potvrdamanif" data-id="${data[i].Id}">Potvrdi manifestaciju</button>`
                              + "</div>";
                $('.card-deck').append(eachManif);
            }

        }
    });

    $('body').on('click', '.potvrdamanif', function () {
            $.ajax({
                url: '/manifestacije-potvrda',
                method: 'GET',
                data: {
                    Id: $(this).attr("data-id")
                },
                success: function () {
                    window.location.href = "index.html";
                }
            });
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