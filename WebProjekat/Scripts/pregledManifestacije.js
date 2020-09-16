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

                var kupovinaKarata = '<select id="tipkarte" name="tipkarte">'
                                        + '<option value="regular">Regular karta</option>'
                                        + '<option value="vip">Vip karta</option>'
                                        + '<option value="fanpit">Fanpit karta</option>'
                                    + '</select>'               
                                    + '<input type="number" min="1" step="1" id="brojKarata" name="brojKarata" placeholder="Broj karata">'
                    + `<button type="button" id="kupi" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">Rezervisi</button>`;
                $('.kupovina').append(kupovinaKarata);
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

    var kupacId;
    var izabranTipKarte;
    var cenaKarteSaPopustom;
    var brojDodatnihBodova;

    $('body').on('click', '#kupi', function () {
        console.log("PROVERA KLIKA");
        var tipKarte = document.getElementById("tipkarte");
        izabranTipKarte = tipKarte.options[tipKarte.selectedIndex].value;

        $.ajax({
            url: '/provera-popusta',
            method: 'GET',
            success: function (data) {
                kupacId = data.Id;
                console.log(data);
                var cenaKarte;
                if (izabranTipKarte === 'regular')
                    cenaKarte = cenaRegularKarte;
                else if (izabranTipKarte === 'vip')
                    cenaKarte = 4 * cenaRegularKarte;
                else
                    cenaKarte = 2 * cenaRegularKarte;

                var ukupnaCena = $('#brojKarata').val() * cenaKarte;
                cenaKarteSaPopustom = cenaKarte - (cenaKarte * (data.Popust / 100));
                var ukupnaCenaSaPopustom = $('#brojKarata').val() * cenaKarteSaPopustom;

                brojDodatnihBodova = cenaKarte / 1000 * 133;

                var potvrdaIspis = '<div class="ispismodal">'
                    + '<p>Rezervisete ' + $('#brojKarata').val() + ' ' + izabranTipKarte + ' karte po ceni od ' + cenaKarteSaPopustom + ' po karti' +'</p>'
                    + '<p>Ukupna cena: ' + ukupnaCenaSaPopustom + '</p>'
                    + `</div>`;

                $('.modal-body').append(potvrdaIspis);
            }
        });
    });

    $('body').on('click', '.odustanak', function () {
        console.log("ODUSTANI KLIK");
        $(".ispismodal").remove();
    });

    $('body').on('click', '.potvrda', function () {
        console.log("POTVRDA KLIK");
        console.log();
        $.ajax({
            url: '/karte',
            method: 'POST',
            data: {
                ManifestacijaId: idParametar,
                DatumVremeManifestacije: datumManif,
                Cena: cenaKarteSaPopustom,
                BrojRezervisanihKarata: $('#brojKarata').val(),
                KupacID: kupacId,
                Tip: izabranTipKarte,
                BrojDodatnihBodova: brojDodatnihBodova
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

    var cenaRegularKarte;
    var datumManif;

    $.ajax({
        url: `/manifestacije/${idParametar}`,
        method: 'GET',
        success: function (data) {
            datumManif = data.DatumVremeOdrzavanja;
            var datum = new Date(data.DatumVremeOdrzavanja);
            var mesec = datum.getMonth() + 1;
            var tipManif = "";

            cenaRegularKarte = data.CenaRegularKarte;

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