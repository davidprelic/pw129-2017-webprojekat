$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data === 'ADMINISTRATOR') {
                var kartice = '<li class="nav-item"><a class="nav-link" href="korisnici.html"> Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajProdavca.html"> Kreiraj prodavca</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="potvrdaManifestacija.html"> Potvrda manifestacija</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
            else if (data === 'KUPAC') {
                var kartice = '<li class="nav-item"><a class="nav-link" href="register.html">Moje karte</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
            else if (data === 'PRODAVAC') {
                var kartice = '<li class="nav-item"><a class="nav-link" href="prodavacManifestacije.html"> Moje manifestacije</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajManifestaciju.html"> Kreiraj manifestaciju</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
        }
    });

    $.ajax({
        url: '/manifestacije',
        method: 'GET',
        success: function (data) {
            var manifestacije = JSON.parse(data);

            // Sortiranje po datumu
            manifestacije.sort(function (a, b) {
                var dateA = new Date(a.DatumVremeOdrzavanja), dateB = new Date(b.DatumVremeOdrzavanja);
                return dateA - dateB;
            });


            for (var i = 0; i < manifestacije.length; i++) {
                var datum = new Date(manifestacije[i].DatumVremeOdrzavanja);
                var mesec = datum.getMonth() + 1;

                var tipManif = "";

                switch (manifestacije[i].Tip) {
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

                var eachManif = `<div class="col-sm-3 trenutnaManif" data-id="${manifestacije[i].Id}" onclick="prikaziManif(this)">`
                                     + '<div class="card text-center border-success">'
                                        + '<div class="card-header">' + tipManif + '</div>'
                                        + '<div class="card-body">'
                                            + `<img class="card-img-top" src="${manifestacije[i].PosterManifestacije}">`
                                            + '<h5 class="card-title">' + manifestacije[i].Naziv + "</h5>"
                                            + '<p class="card-text">' + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</p>"
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



    $('.primeniFiltere').click(function () {
        var sort = document.getElementById("opcijaSort");
        var sortOption = sort.options[sort.selectedIndex].value;

        var filter = document.getElementById("opcijaFilter");
        var filterOption = filter.options[filter.selectedIndex].value;

        $("div.card-deck").remove();

        $.ajax({
            url: '/filtermanifestacije',
            method: 'GET',
            data: {
                Naziv: $('#naziv').val(),
                Mesto: $('#mesto').val(),
                DatumOd: $('#datumOd').val(),
                DatumDo: $('#datumDo').val(),
                CenaOd: $('#cenaOd').val(),
                CenaDo: $('#cenaDo').val(),
                OpcijaSort: sortOption,
                OpcijaFilter: filterOption
            },
            success: function (data) {

                $('header').append('<div class=card-deck></div>');

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

                    var eachManif = `<div class="col-sm-3 trenutnaManif" data-id="${data[i].Id}" onclick="prikaziManif(this)">`
                        + '<div class="card text-center border-success">'
                        + '<div class="card-header">' + tipManif + '</div>'
                        + '<div class="card-body">'
                        + `<img class="card-img-top" src="${data[i].PosterManifestacije}">`
                        + '<h5 class="card-title">' + data[i].Naziv + "</h5>"
                        + '<p class="card-text">' + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</p>"
                        + '</div>'
                        + '</div>'
                        + "</div>";
                    $('.card-deck').append(eachManif);
                }
            },
            error: function (jqXHR) {
                alert("ERROR");
            }
        });
    });

})

function prikaziManif(jednaManif) {
    window.location = "pregledManifestacije.html" + `?id=${$(jednaManif).attr("data-id")}`;
}
