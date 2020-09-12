$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data === 'ADMINISTRATOR') {
                var kartice = '<li class="nav-item"><a class="nav-link" href="korisnici.html">Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajProdavca.html">Kreiraj prodavca</a></li>';
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
                var kartice = '<li class="nav-item"><a class="nav-link" href="register.html">Moje manifestacije</a></li>';
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

    $.ajax({
        url: `/manifestacije/${idParametar}`,
        method: 'GET',
        success: function (data) {
            var datum = new Date(data.DatumVremeOdrzavanja);
            var mesec = datum.getMonth() + 1;
            var tipManif = "";

            switch (data.Tip) {
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

            var manif = '<div class="card mb-3">'
                            + '<div class="row no-gutters">'
                                + '<div class="col-md-4">'
                                    + `<img src="${data.PosterManifestacije}" class="card-img" alt="...">`
                                + '</div>'
                                + '<div class="col-md-8">'
                                    + '<div class="card-body">'
                                        + `<h5 class="card-title">${data.Naziv}</h5>`
                                        + `<p class="card-text">${tipManif}</p>`
                                        + '<p class="card-text">' + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</p>"
                                    + '</div>'
                                + '</div>'
                            + '</div>'
                        + '</div>'
            $('#prikazManif').append(manif);
        },
        error: function (jqXHR) {
            alert("ERROR");
        }
    });


});